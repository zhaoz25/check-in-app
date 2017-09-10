using CheckInApp.messages;
using CheckInApp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckInApp.controller
{
    public class RunServiceTask
    {
        public static bool stateService = true;

        public static async Task runService(string token, CancellationToken tk)
        {
            stateService = true;
            await Task.Run(async () =>
           {
               while (stateService == true)
               {
                   NotificationHandler handler = new NotificationHandler();
                   ResultAndNotification resultAndNotification = new ResultAndNotification();
                    // gửi request lên server và nhận kết quả
                    resultAndNotification = await handler.GetNotification(token);
                    // nếu ko có lỗi -> tới giờ làm việc
                    if (resultAndNotification.result.error == false)
                   {
                       var message = new TickedMessage();
                       message.Message = "Đã tới giở làm việc";
                       message.startTime = resultAndNotification.notification.startTime;
                       message.endTime = resultAndNotification.notification.endTime;

                        // gửi message, lớp nào nhận sẽ hiển thĩ notification
                        Device.BeginInvokeOnMainThread(() =>
                       {
                           MessagingCenter.Send<TickedMessage>(message, "Message");
                       });
                       System.Diagnostics.Debug.WriteLine("Đã tới giở làm việc");
                   }
                   else
                   {
                       System.Diagnostics.Debug.WriteLine("không có giở làm việc");
                   }

                    // Đợi 30p
                    await Task.Delay(60000);
               }
           }, tk);
        }
    }
}
