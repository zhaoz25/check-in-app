﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.messages
{
    public class TickedMessage
    {
        public string Message { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}