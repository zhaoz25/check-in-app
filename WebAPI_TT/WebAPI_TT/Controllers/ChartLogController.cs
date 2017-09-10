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
    public class ChartLogController : ApiController
    {
        public IHttpActionResult GetLogForChart(string token, int month)
        {
            MemoryCache cache = MemoryCache.Default;
            LoginInfo info = (LoginInfo)cache.Get(token);
            // class chứa kết quả
            Result result = new Result();
            List<ChartData> chartDataList = new List<ChartData>();

            if(info != null)
            {
                string employeeID = info.employeeID;
                using (var ctx = new HREntities())
                {
                    // kiểm tra nhân viên có là quản lý phòng ban không?
                    var managedDepartmentIndex = ctx.Database.SqlQuery<Nullable<Int64>>("select ManagedDepartment from HR_WorkingInfo where EmployeeATID={0}", employeeID).ToList();
                    // ko quản lý phòng ban
                    if (managedDepartmentIndex[0] == null)
                    {
                        // truy van lay tong checkin, tong check out, ten nhan vien cua 1 nhan vien
                        string sql = String.Format("select count(case when InOutMode=1 then 1 else null end) as [countCheckIn],count(case when InOutMode=2 then 1 else null end) as [countCheckOut],FirstName,LastName " +
                            "from TA_TimeLog,HR_Employee where TA_TimeLog.EmployeeATID=HR_Employee.EmployeeATID " +
                            "and HR_Employee.EmployeeATID={0} and InOutMode=1 and month([Time])={1} group by HR_Employee.EmployeeATID,FirstName,LastName",employeeID,month);

                        chartDataList = ctx.Database.SqlQuery<ChartData>(sql).ToList();

                        result.error = false;
                        return Ok(new { Result = result, ChartDataList = chartDataList });
                    }
                    else
                    {
                        // truy van lay tong checkin, tong check out, ten nhan vien trong phong ban
                        string sql = String.Format("select count(case when InOutMode=1 then 1 else null end) as [countCheckIn],count(case when InOutMode=2 then 1 else null end) as [countCheckOut],FirstName,LastName " +
                            "from TA_TimeLog,HR_Employee,HR_WorkingInfo where TA_TimeLog.EmployeeATID=HR_Employee.EmployeeATID and HR_WorkingInfo.EmployeeATID=TA_TimeLog.EmployeeATID " +
                            "and DepartmentIndex={0} and month([Time])={1} group by HR_Employee.EmployeeATID,FirstName,LastName", managedDepartmentIndex[0], month);

                        chartDataList = ctx.Database.SqlQuery<ChartData>(sql).ToList();

                        result.error = false;
                        return Ok(new { Result = result, ChartDataList = chartDataList });
                    }
                }
            }
            else
            {
                result.error = true;
                result.errorDetail = "Token đã hết hạn hoặc không đúng";

                return Ok(new { Result = result, ChartDataList = chartDataList });
            }
        }

        public IHttpActionResult GetMonth(string token)
        {
            MemoryCache cache = MemoryCache.Default;
            LoginInfo info = (LoginInfo)cache.Get(token);
            // class chứa kết quả
            Result result = new Result();
            List<int> month = new List<int>();

            if (info != null)
            {
                string employeeID = info.employeeID;
                using (var ctx = new HREntities())
                {
                    // kiểm tra nhân viên có là quản lý phòng ban không?
                    var managedDepartmentIndex = ctx.Database.SqlQuery<Nullable<Int64>>("select ManagedDepartment from HR_WorkingInfo where EmployeeATID={0}", employeeID).ToList();
                    // ko quản lý phòng ban
                    if (managedDepartmentIndex[0] == null)
                    {
                        string sql = String.Format("select month([Time]) from[TA_TimeLog] where EmployeeATID={0} group by month([Time])",employeeID);
                        // truy vấn tháng chấm công
                        month = ctx.Database.SqlQuery<int>(sql).ToList();

                        return Ok(month);
                    }
                    else
                    {
                        string sql = String.Format("select DISTINCT month([Time]) from[TA_TimeLog],HR_WorkingInfo " +
                            "where TA_TimeLog.EmployeeATID=HR_WorkingInfo.EmployeeATID and DepartmentIndex={0} group by month([Time]),TA_TimeLog.EmployeeATID",managedDepartmentIndex[0]);

                        month = ctx.Database.SqlQuery<int>(sql).ToList();

                        return Ok(month);
                    }
                }
            }
            else
            {
                return Ok(month);
            }
        }
    }
}
