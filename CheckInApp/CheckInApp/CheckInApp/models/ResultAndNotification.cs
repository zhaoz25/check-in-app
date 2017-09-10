using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.models
{
    class ResultAndNotification
    {
        public Result result { get; set; }
        public Notification notification { get; set; }

        public ResultAndNotification()
        {
            result = new Result();
            notification = new Notification();
        }
    }
}
