using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.models
{
    public class Employee
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

        public string fullName { get; set; }
        public string joinedDateDay { get; set; }
        public string fromDateDay { get; set; }
        public string toDateDay { get; set; }

        public Employee()
        {
            departmentName = "";
            
        }

        public string getName()
        {
            return firstName + " " + lastName;
        }

        public string getFromDate()
        {
            string date = fromDate?.ToString("dd/MM/yyyy");
            return date;
        }

        public string getToDate()
        {
            string date = toDate?.ToString("dd/MM/yyyy");
            return date;
        }

        public string getJoinedDate()
        {
            string date = JoinedDate?.ToString("dd/MM/yyyy");
            return date;
        }
    }
}
