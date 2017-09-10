using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_TT.Models
{
    public class Notification
    {
        public DateTime date {get;set;}
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}