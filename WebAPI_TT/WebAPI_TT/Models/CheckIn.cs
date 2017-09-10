using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_TT.Models
{
    public class CheckIn
    {
        public string EmployeeATID { get; set; }
        public DateTime Time { get; set; }
        public string MachineSerial { get; set; }
        public Nullable<short> InOutMode { get; set; }
        public Nullable<short> SpecifiedMode { get; set; }
        
        public string token { get; set; }
    }
}