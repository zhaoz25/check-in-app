using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;
using WebAPI_TT.DatabaseContext;
using WebAPI_TT.Models;
using System.ServiceModel.Channels;


namespace WebAPI_TT.Controllers
{
    public class EmployeeListController : ApiController
    {
        public IHttpActionResult GetListOfEmployees(string token)
        {
            MemoryCache cache = MemoryCache.Default;
            LoginInfo info = (LoginInfo)cache.Get(token);
            //class chứa thông tin nhân viên sẽ hiển thị
            EmployeeInfo emp = new EmployeeInfo();
            // class chứa kết quả
            Result result = new Result();

            if (info != null)
            {
                // lấy thông tin nhóm nhân viên lưu trong cache
                var employeeInfoList = cache.Get(token+"manager");
                // nếu có dữ liệu trong cache thì hiển thị ko cần truy vấn database
                if(employeeInfoList != null)
                {
                    result.error = false;

                    return Ok(new { result, employeeInfoList });
                }
                // --------------------------  //
                string employeeID = info.employeeID;
                using (var ctx = new HREntities())
                {
                    // kiểm tra nhân viên có là quản lý phòng ban không?
                    var managedDepartmentIndex = ctx.Database.SqlQuery<Nullable<Int64>>("select ManagedDepartment from HR_WorkingInfo where EmployeeATID={0}", employeeID).ToList();
                    // ko quản lý phòng ban
                    if (managedDepartmentIndex[0] == null)
                    {
                        // lấy thông tin của nhân viên đó
                        employeeInfoList = ctx.Database.SqlQuery<EmployeeInfo>("select HR_Employee.EmployeeATID,EmployeeCode,FirstName,LastName,FromDate,ToDate,DepartmentIndex,ManagedDepartment " +
                            "from HR_Employee,HR_WorkingInfo where HR_Employee.EmployeeATID=HR_WorkingInfo.EmployeeATID and HR_Employee.EmployeeATID={0}", employeeID).ToList();
                        // lưu thông tin nhân viên vào cache
                        cache.Add(token + "manager", employeeInfoList, DateTimeOffset.UtcNow.AddMinutes(60));
                        
                        result.error = false;
                        return Ok(new { result, employeeInfoList });
                    }
                    else
                    {
                        // lấy thông tin tất cả nhân viên của phòng ban
                        employeeInfoList = ctx.Database.SqlQuery<EmployeeInfo>("select HR_Employee.EmployeeATID,EmployeeCode,FirstName,LastName,FromDate,ToDate,DepartmentIndex,ManagedDepartment " +
                            "from HR_Employee,HR_WorkingInfo where HR_Employee.EmployeeATID=HR_WorkingInfo.EmployeeATID and DepartmentIndex={0}", managedDepartmentIndex[0]).ToList();
                        // lưu thông tin nhóm nhân viên vào cache
                        cache.Add(token + "manager", employeeInfoList, DateTimeOffset.UtcNow.AddMinutes(60));

                        result.error = false;
                        return Ok(new { result, employeeInfoList });
                    }
                }
            }
            else
            {
                result.error = true;
                result.errorDetail = "Token đã hết hạn hoặc không đúng";

                return Ok(result);
            }
        }

        public IHttpActionResult GetIp()
        {
            return Ok(Request.GetClientIpAddress());
        }

        private string GetClientIp(HttpRequestMessage request)
        {
            // Web-hosting
            if (request.Properties.ContainsKey(Models.HttpRequestMessageExtensions.HttpContext))
            {
                HttpContextWrapper ctx =
                    (HttpContextWrapper)request.Properties[Models.HttpRequestMessageExtensions.HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            // Self-hosting
            if (request.Properties.ContainsKey(Models.HttpRequestMessageExtensions.RemoteEndpointMessage))
            {
                RemoteEndpointMessageProperty remoteEndpoint =
                    (RemoteEndpointMessageProperty)request.Properties[Models.HttpRequestMessageExtensions.RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            // Self-hosting using Owin
            if (request.Properties.ContainsKey(Models.HttpRequestMessageExtensions.OwinContext))
            {
                dynamic owinContext = request.Properties[Models.HttpRequestMessageExtensions.OwinContext];
                if (owinContext != null)
                {
                    return owinContext.Request.RemoteIpAddress;
                }
            }

            return null;
        }

    }
}
