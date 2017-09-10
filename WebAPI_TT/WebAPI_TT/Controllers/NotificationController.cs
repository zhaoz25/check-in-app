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
    public class NotificationController : ApiController
    {
        public IHttpActionResult GetNotification(string token)
        {
            MemoryCache cache = MemoryCache.Default;
            LoginInfo info = (LoginInfo)cache.Get(token);
            Result result = new Result();

            if(info != null)
            {
                string employeeID = info.employeeID;

                using(var cx = new HREntities())
                {
                    // lấy ngày hiện tại
                    DateTime currentDate = DateTime.Now;
                    //lấy thông tin ngày làm việc
                    var notification = cx.Database.SqlQuery<Notification>("select [Date],StartTime,EndTime " +
                        "from TA_EmployeeSpecialShiftDetail where EmployeeATID={0} ",employeeID).ToList();
                    // kiểm tra có thông tin ngày làm việc hay không?
                    if(notification.Count == 0)
                    {
                        result.error = true;
                        result.errorDetail = "Không thể lấy thông tin ngày làm việc";
                        return Ok(result);
                    }
                    // kiểm tra lịch làm việc ngày hôm nay
                    for (int i = 0; i < notification.Count; i++) {
                        // kiểm tra ngày
                        if (notification[i].date.Date.CompareTo(currentDate.Date) == 0)
                        {
                            // lấy tổng giây startTime trong csdl
                            double totalSecondsNotification = notification[i].startTime.TimeOfDay.TotalSeconds;
                            // lấy tổng giây thời gian hiện tại
                            double totalSecondsCurrent = currentDate.TimeOfDay.TotalSeconds;
                            double rs = totalSecondsNotification - totalSecondsCurrent;
                            // lấy tổng giây csdl trừ tổng giây hiện tại. Nếu <= 1800s (30p) và > 600s (10p) thì trả vể kq
                            // -> trước thời gian làm việc 30p và trễ khoảng 10p thì thông báo
                            if ((totalSecondsNotification - totalSecondsCurrent) <= 1800 && (totalSecondsNotification - totalSecondsCurrent) > -600)
                            {
                                result.error = false;
                                return Ok(new { Result = result, Notification = notification[i] });
                            }
                            
                             
                        }
                    }
                    result.error = true;
                    return Ok(result);

                }
            }
            else
            {
                result.error = true;
                result.errorDetail = "Token đã hết hạn hoặc không đúng!";
                return Ok(result);
            }


        }
    }
}
