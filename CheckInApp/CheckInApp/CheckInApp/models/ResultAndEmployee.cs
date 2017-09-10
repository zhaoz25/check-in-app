using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.models
{
    class ResultAndEmployee
    {
        public Result result { get; set; }
        public Employee employee { get; set; }
        public List<Employee> employeeInfoList { get; set; }

        public ResultAndEmployee()
        {
            result = new Result();
            employee = new Employee();

            employeeInfoList = new List<Employee>();
        }
    }
}
