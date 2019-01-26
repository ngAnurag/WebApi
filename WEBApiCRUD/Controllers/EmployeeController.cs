using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccessLayer;
using System.Data.SqlClient;

namespace WEBApiCRUD.Controllers
{
    public class EmployeeController : ApiController
    {
        public IEnumerable<Employee> GET()
        {
            using (QueriesEntities QE = new QueriesEntities())
            {
                return QE.Employees.ToList();
            }
        }

        public HttpResponseMessage GET(int ID)
        {
            try
            {
                using (QueriesEntities QE = new QueriesEntities())
                {
                    var record = QE.Employees.FirstOrDefault(e => e.ID == ID);
                    if (record != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, record);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found for " + ID.ToString());
                    }
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Error while fnding record.");
            }

        }

        public HttpResponseMessage POST(Employee emp)
        {
            try
            {
                using (QueriesEntities QE = new QueriesEntities())
                {
                    QE.Employees.Add(emp);
                    QE.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, emp);
                    message.Headers.Location = new Uri(Request.RequestUri + emp.ID.ToString());
                    return message;
                }
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error while adding new record.");
            }
        }

        public IHttpActionResult Put([FromBody] Employee emp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");

            using (var ctx = new QueriesEntities())
            {
                var existingemp = ctx.Employees.Where(s => s.ID == emp.ID).FirstOrDefault<Employee>();

                if (existingemp != null)
                {
                    existingemp.FirstName = emp.FirstName;
                    existingemp.LastName = emp.LastName;
                    existingemp.Gender = emp.Gender;
                    existingemp.Salary = emp.Salary;

                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok();
        }

        //public HttpResponseMessage PUT(int ID)
        //{
        //    try
        //    {
        //        using (QueriesEntities QE = new QueriesEntities())
        //        {
        //            if (QE.Employees.FirstOrDefault(e => e.ID == ID) != null)
        //            {
        //                var entities = QE.Employees.FirstOrDefault(e => e.ID == ID);
        //                entities.FirstName = emp.FirstName;
        //                entities.LastName = emp.LastName;
        //                entities.Gender = emp.Gender;
        //                entities.Salary = emp.Salary;
        //                QE.SaveChanges();
        //                return Request.CreateResponse(HttpStatusCode.OK, "Record updtaed for ID" + ID.ToString());
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found for ID" + ID.ToString());
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error while deleting existing record.");
        //    }
        //}

        public HttpResponseMessage DELETE(int ID)
        {
            try
            {
                using (QueriesEntities QE = new QueriesEntities())
                { 
                    if (QE.Employees.FirstOrDefault(e => e.ID == ID) != null)
                    {
                        QE.Employees.Remove(QE.Employees.FirstOrDefault(e => e.ID == ID));
                        QE.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Record deleted for ID" + ID.ToString());
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found for ID" + ID.ToString());
                    }
                }
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error while deleting existing record.");
            }
        }
    }
}
