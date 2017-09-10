using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;
using WebAPI_TT.DatabaseContext;
using WebAPI_TT.Models;

namespace WebAPI_TT.Controllers
{
    public class EmployeeInfoController : ApiController
    {
        // GET api/values  Gửi reset code phục hồi mật khẩu
        public IHttpActionResult GetUserInfo(string token)
        {
            MemoryCache cache = MemoryCache.Default;
            LoginInfo info = (LoginInfo)cache.Get(token);
            //class chứa thông tin nhân viên sẽ hiển thị
            EmployeeInfo emp = new EmployeeInfo();

            Result result = new Result();

            if(info != null)
            {
                // lấy employeeid của tài khoản
                string employeeID = info.employeeID;
                using(var ctx = new HREntities())
                {
                    // lấy thông tin nhân viên (EmployeeAtid,ECode,Name,FromDate,Todate,DepartmentIndex,Managed) từ 2 table HR_Employee và HR_workingInfo
                    var employeeInfo = ctx.Database.SqlQuery<EmployeeInfo>("select HR_Employee.EmployeeATID,HR_Employee.EmployeeCode,HR_Employee.FirstName,HR_Employee.LastName," +
                        "HR_WorkingInfo.FromDate,HR_WorkingInfo.ToDate,HR_WorkingInfo.DepartmentIndex,HR_WorkingInfo.ManagedDepartment,JoinedDate " +
                        "from HR_Employee,HR_WorkingInfo " +
                        "where HR_Employee.EmployeeATID=HR_WorkingInfo.EmployeeATID and HR_Employee.EmployeeATID={0}",employeeID).ToList();
                    // kiểm tra có thông tin của nhân viên ?
                    if(employeeInfo.Count == 0)
                    {
                        result.error = true;
                        result.errorDetail = "Không tìm thấy nhân viên";
                        return Ok(new { Result=result,Employee=emp });
                    }
                    // lấy tên phòng ban 
                    long departmentIndex = employeeInfo[0].departmentIndex;
                    var departmentName = ctx.Database.SqlQuery<string>("select Name from HR_Department where [Index]={0}", departmentIndex).ToList();
                    // gán thông tin vào class emp
                    emp = employeeInfo[0];
                    // gán tên phòng ban
                    emp.departmentName = departmentName[0];

                    result.error = false;
                    return Ok(new { Result=result,Employee=emp });

                }
            }
            else
            {
                result.error = true;
                result.errorDetail = "Token đã hết hạn hoặc không đúng !";

                return Ok(new { Result=result,Employee=emp });
            }
        }
        
    }
}
