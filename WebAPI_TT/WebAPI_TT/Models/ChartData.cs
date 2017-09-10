using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_TT.Models
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
        
    }
}