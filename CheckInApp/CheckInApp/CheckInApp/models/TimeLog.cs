using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.models
{
    class TimeLog
    {
        public string EmployeeATID { get; set; }
        public DateTime Time { get; set; }
        public string MachineSerial { get; set; }
        public Nullable<short> InOutMode { get; set; }
        public Nullable<short> SpecifiedMode { get; set; }

        public string inOut { get; set; }
        public string timeFormat { get; set; }
        public string color { get; set; }

        public TimeLog()
        {
            inOut = "";
        }

        public void getInOut()
        {
            if(InOutMode == 1)
            {
                inOut = "Vào";
                color = "LightGreen";
            }
            else
            {
                inOut = "Ra";
                color = "Orange";
            }
        }

        public void getTime()
        {
            timeFormat = Time.ToString("dd/MM/yy HH:mm");
        }
    }
}
