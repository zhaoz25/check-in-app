using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Runtime.Caching;
using System.Web.Http;
using WebAPI_TT.DatabaseContext;
using WebAPI_TT.Models;
using WebAPIDemo;

namespace WebAPI_TT.Controllers
{
    public class UserController : ApiController
    {

        // GET api/values  Gửi reset code phục hồi mật khẩu
        public IHttpActionResult GetSendResetCode(string email)
        {
            Result result = new Result();
            // tạo resetcode ngẫu nhiên
            string resetCode = Guid.NewGuid().ToString();
			resetCode = resetCode.Substring(0, 5);
            // Gửi email
            MyEmail mailMessenger = new MyEmail();
            string content = "Bạn đã gửi yêu cầu phục hồi mật khẩu. Nhập đoạn code sau vào để thay đổi mật khẩu:"+resetCode;
            bool check = mailMessenger.SendEmail("phuchoang2510@gmail.com", email, "Phục hồi mật khẩu", content);
            // không gửi được email
            if (check == false)
            {
                result.error = true;
                result.errorDetail = "Địa chỉ email không đúng";
                return Ok(result);
            }
            else
            {
                //string a = Guid.NewGuid().ToString();

                // nếu gửi email thành công -> update resetcode cho tài khoản
                using (var ctx = new HREntities())
                {
                    int row = ctx.Database.ExecuteSqlCommand("update HR_EmployeeSystem set ResetPasswordCode ={0} where AccountName={1} ",
                        resetCode, email);

                    if (row == 0)
                    {
                        result.error = true;
                        result.errorDetail = "Địa chỉ email không đúng";
                        return Ok(result);
                    }
                    else
                    {
                        result.error = false;
                        return Ok(result);
                    }
                }
            }
        }
        // GET api    Đăng xuất
        public IHttpActionResult GetLogout(string token)
        {
            Result result = new Result();
            MemoryCache cache = MemoryCache.Default;
            var info = cache.Get(token);

            if(info != null)
            {
                cache.Remove(token);
            }

            result.error = false;
            result.errorDetail = "Đã xóa thông tin người dùng";

            return Ok(result);

        }
        // POST api/values   Đăng nhập và gửi lại token cho device
        public IHttpActionResult PostLogin([FromBody]LoginInfo info)
        {
            Result result = new Result();
            MemoryCache cache = MemoryCache.Default;

            using (var ctx = new HREntities())
            {
                var employeeList = ctx.HR_EmployeeSystem.SqlQuery("select * from HR_EmployeeSystem where AccountName={0} and Password={1}",
                    info.username, info.password).ToList();

                if (employeeList.Count == 0)
                {
                    result.error = true;
                    result.token = "";

                    return Ok(result);
                }
                else
                {
                    // lấy employeeID của tài khoản lưu vào cache
                    info.employeeID = employeeList[0].EmployeeATID;

                    result.error = false;
                    result.token = JwtManager.GenerateToken(info.username, 10);
                    // lưu token vào bộ nhớ cache trong 60 phút
                    cache.Add(result.token,info,DateTimeOffset.UtcNow.AddMinutes(60));
                    Debug.WriteLine(result.token);
                    return Ok(result);
                }
            }
        }
        
        // PUT api/values   Thay đổi mật khẩu
        public IHttpActionResult PutChangePassword(LoginInfo info)
        {
            Result result = new Result();
            // nếu action là change password
            if (info.resetCode.Equals("") == true)
            {
                using (var ctx = new HREntities())
                {
                    int row = ctx.Database.ExecuteSqlCommand("update HR_EmployeeSystem set Password ={0} where AccountName={1} ",
                        info.newPassword, info.username);

                    if (row == 0)
                    {
                        result.error = true;
                        return Ok(result);
                    }
                    else
                    {
                        result.error = false;

                        return Ok(result);
                    }
                }
            }
            else // nếu action là update forget password
            {
                using (var ctx = new HREntities())
                {
                    int row = ctx.Database.ExecuteSqlCommand("update HR_EmployeeSystem set Password ={0} where AccountName={1} and ResetPasswordCode={2} ",
                        info.newPassword, info.username,info.resetCode);
                    
                    if (row == 0)
                    {
                        result.error = true;
                        result.errorDetail = "Email hoặc code không đúng!";

                        return Ok(result);
                    }
                    else
                    {
                        // xóa resetcode của tài khoản
                        if(removeResetCode(info.username) == true)
                        {
                            result.error = false;
                            return Ok(result);
                        }
                        else
                        {
                            result.error = false;
                            result.errorDetail = "Không thể xóa resetcode";

                            return Ok(result);
                        }

                        
                    }
                }
            }
        }

        private bool removeResetCode(string accountName)
        {
            using (var ctx = new HREntities())
            {
                int row = ctx.Database.ExecuteSqlCommand("update HR_EmployeeSystem set ResetPasswordCode ={0} where AccountName={1}",
                    "zzz.",accountName);

                if (row == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


    }
}
