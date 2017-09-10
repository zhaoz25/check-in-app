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
    public class TimeLogController : ApiController
    {
        public IHttpActionResult Get(string token,string employeeID)
        {
            MemoryCache cache = MemoryCache.Default;
            LoginInfo info = (LoginInfo)cache.Get(token);
            var timeLogList = new List<TimeLog>();
            // class chứa kết quả
            Result result = new Result();
            List<int> month = new List<int>();

            if(info != null)
            {
                using (var ctx = new HREntities())
                {
                    timeLogList = ctx.Database.SqlQuery<TimeLog>
                        ("select [Time],MachineSerial,InOutMode,SpecifiedMode from TA_TimeLog where EmployeeATID={0} order by [Time] DESC", employeeID).ToList();
                    // truy vấn tháng chấm công
                    month = ctx.Database.SqlQuery<int>("select month([Time]) from[TA_TimeLog] where EmployeeATID={0} group by month([Time])",employeeID).ToList();
                    if (timeLogList.Count > 0)
                    {
                        result.error = false;
                        return Ok(new { result = result, TimeLogList = timeLogList, Month = month });
                    }
                    else
                    {
                        result.error = false;
                        result.errorType = 2;

                        return Ok(new { result = result, TimeLogList = timeLogList , Month = month});
                    }
                }
            }
            else
            {
                result.error = true;
                result.errorDetail = "Token đã hết hạn";
                result.errorType = 1;

                return Ok(new { result = result, TimeLogList = timeLogList, Month = month });
            }
        }
        public IHttpActionResult GetLogByMonth(string token, string employeeID,int month)
        {
            MemoryCache cache = MemoryCache.Default;
            LoginInfo info = (LoginInfo)cache.Get(token);
            var timeLogList = new List<TimeLog>();
            // class chứa kết quả
            Result result = new Result();

            if (info != null)
            {
                using (var ctx = new HREntities())
                {
                    timeLogList = ctx.Database.SqlQuery<TimeLog>
                        ("select [Time],MachineSerial,InOutMode,SpecifiedMode from TA_TimeLog where EmployeeATID={0} " +
                        "and month([Time])={1} order by [Time] DESC", employeeID,month).ToList();
                    if (timeLogList.Count > 0)
                    {
                        result.error = false;
                        return Ok(new { result = result, TimeLogList = timeLogList });
                    }
                    else
                    {
                        result.error = false;
                        result.errorType = 2;

                        return Ok(new { result = result, TimeLogList = timeLogList });
                    }
                }
            }
            else
            {
                result.error = true;
                result.errorDetail = "Token đã hết hạn";
                result.errorType = 1;

                return Ok(new { result = result, TimeLogList = timeLogList });
            }
        }
    }
}
