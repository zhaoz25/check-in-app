using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.models
{
    class CheckIn
    {
        public string EmployeeATID { get; set; }
        public DateTime Time { get; set; }
        public string MachineSerial { get; set; }
        public Nullable<short> InOutMode { get; set; }
        public Nullable<short> SpecifiedMode { get; set; }

        public string token { get; set; }

        public CheckIn()
        {
            EmployeeATID = "";
            MachineSerial = "";
        }
    }
}
