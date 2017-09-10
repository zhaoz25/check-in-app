using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.models
{
    public class ChartData
    {
        public int countCheckIn { get; set; }
        public int countCheckOut { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }


        public ChartData()
        {

        }

        public string getName()
        {
            string fullName = firstName + lastName;
            return fullName;
        }
    }
}
