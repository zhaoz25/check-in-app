using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using CheckInApp.messages;
using Android.Content;
using CheckInApp.Droid.services;
using SharedCheckInApp;

using CheckInApp.Droid;


namespace CheckInApp.Droid
{
    [Activity(Label = "CheckInApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            
            global::Xamarin.Forms.Forms.Init(this, bundle);
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();

            LoadApplication(new App());
       
            //WireUpLongRunningTask();
            // đăng ký nhận message chạy service từ portable 
            MessagingCenter.Subscribe<StartTaskGetNotification>(this, "StartTask", message => {
                // chạy service từ class GetNotificationTaskService
                //var intent2 = new Intent(this, typeof(GetNotificationTaskService));
               // StopService(intent2);

                var intent = new Intent(this, typeof(GetNotificationTaskService));
                Bundle b = new Bundle();
                b.PutString("token", message.token);
                intent.PutExtra("data",b);

                StartService(intent);
                System.Diagnostics.Debug.WriteLine("vao service");
            });
            // đăng ký nhận message tắt service từ portable 
            MessagingCenter.Subscribe<StopTaskGetNotification>(this, "StopTask", message =>
            {
                var intent = new Intent(this, typeof(GetNotificationTaskService));
                StopService(intent);
            });

            // nhận message từ service và push notification 
            MessagingCenter.Subscribe<TickedMessage>(this, "Message", message => {
                Device.BeginInvokeOnMainThread(() => {
                    var nMgr = (NotificationManager)GetSystemService(NotificationService);
                    var notification = new Notification(Resource.Drawable.icon, "Message from demo service");
                    var pendingIntent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0);
                    notification.SetLatestEventInfo(this, "Thời gian làm việc hôm nay", 
                        message.startTime.ToString("HH:mm")+" - "+message.endTime.ToString("HH:mm"), pendingIntent);
                    Random r = new Random();
                    int id = r.Next(1,100000);
                    nMgr.Notify(id, notification);
                });
            });
        }

        void WireUpLongRunningTask()
        {
            MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", message => {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message => {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StopService(intent);
            });
        }

       

    }
}

