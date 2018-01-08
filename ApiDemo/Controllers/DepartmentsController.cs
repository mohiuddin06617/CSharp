using ApiRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiDemo.Controllers
{
    [RoutePrefix("api/Departments")]
    [BasicAuthentication]
    //[EnableCors("*", "*", "*")]
    public class DepartmentsController : ApiController
    {
        DataContext context = new DataContext();

        /*
            /Departments
         */
        [Route("")]
        
        [RoleAuthorization]
        public IHttpActionResult Get()
        {
            List<Department> dlist = context.Set<Department>().ToList();
            List<Department> deptlist = new List<Department>();
            foreach(Department d in dlist)
            {
                d.Links = new List<Link>()
                {
                    new Link(){
                        Href = Url.Link("EmployeeList", new {id=d.Id}),
                        Rel = "employees"
                    }
                };
                deptlist.Add(d);
            }
            //return Content(HttpStatusCode.OK, "Welcome");
            return Ok(deptlist);
        }

        /*
            /Departments/5
         */
        [Route("{id}", Name="FindDepartment")]
        [AllowAnonymous]
        public IHttpActionResult Get([FromUri]int id)
        {
            Department dept = context.Set<Department>().Find(id);
            if(dept == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(dept);
        }

        /*
            /Departments
         */
        [Route("")]
        public IHttpActionResult Post([FromBody]Department dept)
        {

            context.Set<Department>().Add(dept);
            context.SaveChanges();
            return Created(Url.Link("FindDepartment", new { id = dept.Id}), dept);
            //return Content(HttpStatusCode.Created, Url.Link("FindDepartment", new {id = dept.Id}));
        }

        /*
            /Departments/5
         */
        [Route("{id}")]
        [DisableCors]
        public IHttpActionResult Put([FromBody]Department dept, [FromUri]int id)
        {
            dept.Id = id;
            context.Entry<Department>(dept).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return Ok(dept);
        }

        /*
            /Departments/5
         */
        [Route("{id}")]
        public IHttpActionResult Delete([FromUri]int id)
        {
            Department dept = context.Set<Department>().Find(id);
            if(dept != null)
            {
                context.Set<Department>().Remove(dept);
                context.SaveChanges();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /*
            /Departments/Search/deptname
         */
        [Route("Search/{name}")]
        public IHttpActionResult GetDepartmentByName([FromUri]string name)
        {
            
            List<Department> deptlist = context.Set<Department>().Where(d => d.Name.Contains(name)).ToList();
            return Ok(deptlist);
        }

        /*
            /Departments/5/Employees
         */
        [Route("{id}/Employees", Name="EmployeeList")]
        public IHttpActionResult GetEmployees([FromUri]int id)
        {
            List<Employee> emplist = context.Set<Employee>().Where(e => e.DepartmentId == id).ToList();
            return Ok(emplist);
        }
    }
}
