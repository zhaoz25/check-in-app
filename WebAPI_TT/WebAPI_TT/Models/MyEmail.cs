using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebAPI_TT.Models
{
    public class MyEmail
    {
        public bool SendEmail(string from, string to,string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage(from, to);

                mail.From = new MailAddress(from);
                mail.Subject = subject;
                string Body = body;
                mail.Body = Body;

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential("phuchoang2510@gmail.com", "159073561");

                smtp.EnableSsl = true;
                smtp.Send(mail);

                return true;
            }catch(Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
            
        }
    }
}