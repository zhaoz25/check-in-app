using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.messages
{
    public class StartTaskGetNotification
    {
        public string token { get; set; }

        public StartTaskGetNotification(string token)
        {
            this.token = token;
        }
    }
}
