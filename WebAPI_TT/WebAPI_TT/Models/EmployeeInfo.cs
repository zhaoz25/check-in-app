using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_TT.Models
{
    public class EmployeeInfo
    {
        public string employeeATID { get; set; }
        public string employeeCode { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public Nullable<DateTime> fromDate { get; set; }
        public Nullable<DateTime> toDate { get; set; }
        public long departmentIndex { get; set; }
        public Nullable<long> managedDepartment { get; set; }
        public Nullable<DateTime> JoinedDate { get; set; }

        public string departmentName { get; set; }

        public EmployeeInfo()
        {
            departmentName = "";
        }
    }
}