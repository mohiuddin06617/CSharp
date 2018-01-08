using ApiRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiDemo.Controllers
{
    public class EmployeesController : ApiController
    {
        DataContext context = new DataContext();

        /*
            /Departments
         */
        public IHttpActionResult Get()
        {
            //return Content(HttpStatusCode.OK, "Welcome");
            return Ok(context.Set<Employee>().ToList());
        }

        /*
            /Departments/5
         */
        public IHttpActionResult Get([FromUri]int id)
        {
            Employee emp = context.Set<Employee>().Find(id);
            if (emp == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(emp);
        }

        /*
            /Employees
         */
        public IHttpActionResult Post([FromBody]Employee emp)
        {
            context.Set<Employee>().Add(emp);
            context.SaveChanges();
            return Created("/Employees/" + emp.Id, emp);
        }

        /*
            /Employees/5
         */
        public IHttpActionResult Put([FromBody]Employee emp, [FromUri]int id)
        {
            emp.Id = id;
            context.Entry<Employee>(emp).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return Ok(emp);
        }

        /*
            /Employees/5
         */
        public IHttpActionResult Delete([FromUri]int id)
        {
            Employee emp = context.Set<Employee>().Find(id);
            if (emp != null)
            {
                context.Set<Employee>().Remove(emp);
                context.SaveChanges();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /*
            /Employees/Search/empname
         */
        [Route("api/Employees/Search/{name}")]
        public IHttpActionResult GetEmployeeByName([FromUri]string name)
        {
            List<Employee> emplist = context.Set<Employee>().Where(d => d.Name.Contains(name)).ToList();
            return Ok(emplist);
        }

        /*
            /Employees/5/Department
         */
        [Route("api/Employees/{id}/Department")]
        public IHttpActionResult GetDepartment([FromUri]int id)
        {
            Employee emp = context.Set<Employee>().Find(id);
            Department dept = context.Set<Department>().Find(emp.DepartmentId);
            return Ok(dept);
        }
    }
}
