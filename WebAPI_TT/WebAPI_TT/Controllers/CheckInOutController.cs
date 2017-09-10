using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using WebAPI_TT.DatabaseContext;
using WebAPI_TT.Models;

namespace WebAPI_TT.Controllers
{
    public class CheckInOutController : ApiController
    {
        // POST api/values   Đăng nhập và gửi lại token cho device
        public IHttpActionResult PostCheckIn([FromBody]CheckIn checkIn)
        {
            MemoryCache cache = MemoryCache.Default;
            LoginInfo info = (LoginInfo)cache.Get(checkIn.token);
            Result result = new Result();

            if (info != null)
            {
                using (var ctx = new HREntities())
                {
                    // lấy địa chỉ trong database
                    var ipAddress = ctx.Database.SqlQuery<string>("select IPAddress from MS_ValidLocationList").ToList();
                    // lấy địa chỉ ip của request
                    string requestIP = Request.GetClientIpAddress();
                    // so sánh request với ip trong database
                    bool checkIP = false;
                    for(int i = 0; i < ipAddress.Count; i++)
                    {
                        if(requestIP.Equals(ipAddress[i]) == true)
                        {
                            checkIP = true;
                            break;
                        }
                    }
                    
                    // sai ip
                    if (checkIP == false)
                    {
                        result.error = true;
                        result.errorDetail = "Không đúng địa chỉ ip";
                        result.errorType = 2;
                        return Ok(result);
                    }

                    DateTime time = DateTime.Now;
                    int row = ctx.Database.ExecuteSqlCommand("insert into TA_TimeLog(EmployeeATID,[Time],InOutMode,SpecifiedMode,MachineSerial) values({0},{1},{2},{3},{4})"
                        , info.employeeID, time, checkIn.InOutMode,checkIn.SpecifiedMode,checkIn.MachineSerial);

                    if(row == 0)
                    {
                        result.error = true;
                        result.errorDetail = "Không thể check in. Hãy thử lại sau.";
                        result.errorType = 3;
                        return Ok(result);
                    }
                    else
                    {
                        result.error = false ;
                        result.time = time.ToString("dd/MM/yyyy HH:mm");
                        return Ok(result);
                    }
                }
            }
            else
            {
                result.error = true;
                result.errorDetail = "Token đã hết hạn. Hãy đăng nhập lại.";
                result.errorType = 1;
                return Ok(result);
            }
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
