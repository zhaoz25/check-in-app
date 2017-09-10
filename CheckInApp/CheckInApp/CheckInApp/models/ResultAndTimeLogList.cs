using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.models
{
    class ResultAndTimeLogList
    {
        public Result result { get; set; }
        public List<TimeLog> TimeLogList { get; set; }
        public List<int> month { get; set; }

        public ResultAndTimeLogList()
        {
            result = new Result();
            TimeLogList = new List<TimeLog>();
            month = new List<int>();
        }
    }
}
