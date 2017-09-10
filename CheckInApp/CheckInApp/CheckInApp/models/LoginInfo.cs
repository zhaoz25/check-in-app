﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.models
{
    class LoginInfo
    {
        public string username { get; set; }
        public string password { get; set; }
        public string newPassword { get; set; }
        public string resetCode { get; set; }
        public string employeeID { get; set; }

        public LoginInfo()
        {
            username = "";
            password = "";
            newPassword = "";
            resetCode = "";
            employeeID = "";
        }
    }
}