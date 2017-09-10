using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using CheckInApp.controller;

namespace CheckInApp.Droid.services
{
    [Service]
    class GetNotificationTaskService : Service
    {
        CancellationTokenSource _cts;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();
            Bundle extra = intent.GetBundleExtra("data");
            string token = extra.GetString("token");

            Task.Run(() =>
            {
                try
                {
                    // chạy phương thức bất đồng bộ từ portable
                    //RunServiceTask runServiceTask = new RunServiceTask();
                    RunServiceTask.runService(token,_cts.Token).Wait();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                
            }, _cts.Token);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            // ngừng vòng lập while() ở class RunServiceTask
            //RunServiceTask.stateService = false;
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();

                _cts.Cancel();
            }
            base.OnDestroy();
        }
    }
}