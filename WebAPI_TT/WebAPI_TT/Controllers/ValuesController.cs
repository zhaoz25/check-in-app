using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI_TT.DatabaseContext;
using WebAPI_TT.Models;

namespace WebAPI_TT.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IHttpActionResult Get()
        {
            HR_EmployeeSystem hr = new HR_EmployeeSystem();
            return Ok(hr);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public IHttpActionResult PostLogin([FromBody]LoginInfo info)
        {
            using (var ctx = new HREntities())
            {
                List<HR_EmployeeSystem> employeeList = ctx.HR_EmployeeSystem.SqlQuery("select * from HR_EmployeeSystem where AccountName={0} and Password={1}", info.username,info.password).ToList();

                if (employeeList == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(employeeList);
                }
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
