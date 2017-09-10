using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.models
{
    public class Result
    {
        public bool error { get; set; }
        public string token { get; set; }
        public string errorDetail { get; set; }
        public int errorType { get; set; }
        public string time { get; set; }

        public Result()
        {
            error = true;
            token = "";
            errorDetail = "";
            errorType = 0;
            time = "";
        }
    }
}
