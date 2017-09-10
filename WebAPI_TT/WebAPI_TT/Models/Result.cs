using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_TT.Models
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
            error = false;
            token = "";
            errorDetail = "";
            errorType = 0;
            time = "";
        }
    }
}