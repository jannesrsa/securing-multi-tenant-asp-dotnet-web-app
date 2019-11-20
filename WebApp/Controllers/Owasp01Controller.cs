using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using WebApp.Models;

namespace WebApp.Controllers
{

    /// <summary>
    /// OWASP 02 Sql Injection Attacks
    /// </summary>
    public class Owasp01Controller : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult CustomerSearch()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CustomerSearchBroken(string searchText)
        {
            var customers = new List<Customer>();
            using (var sqlConnection =
                new SqlConnection
                (ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                sqlConnection.Open();
                string sqlSelect =
                    $@"SELECT CustomerId,FirstName,LastName
                        FROM Customer
                        WHERE CustomerId = " +
                            Convert.ToInt32(searchText);
                using (var sqlCommand = new SqlCommand
                    (sqlSelect, sqlConnection))
                {
                    using (SqlDataReader reader =
                        sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            return View("CustomerList", customers);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CustomerSearch(string searchText)
        {
            var customers = new List<Customer>();
            using (var sqlConnection =
                new SqlConnection
                (ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                sqlConnection.Open();

                string sqlSelect =
                    $@"SELECT Id,FirstName,LastName
                        FROM Customer
                        WHERE Id = @CustomerId";
                using (var sqlCommand =
                    new SqlCommand(sqlSelect, sqlConnection))
                {
                    int customerId =
                        int.TryParse(searchText, out customerId) ?
                        int.Parse(searchText) : 0;

                    sqlCommand.Parameters.
                        AddWithValue("@CustomerId", customerId);
                    using (SqlDataReader reader =
                        sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            return View("CustomerList", customers);
        }

        public ActionResult Owasp02SqlInjectionList()
        {
            List<Customer> customers;
            using (var context = new ApplicationDbContext())
            {
                customers = context.Customers.ToList();
            }

            return View("02SqlInjection/CustomerList", customers);
        }



        // OWASP 03 Problem Auth

        public ActionResult Owasp03ProblemAuth()
        {
            string sessionId = Session.SessionID;
            Session["MySecret"] = "My deep dark secret here. DO NOT SHARE!";
            return Redirect("/Owasp/Owasp03LandingPage?id=" + sessionId);
        }


        public ActionResult Owasp03Secret(string id)
        {

            return View("/Owasp/Owasp03LandingPage");
        }

        public ActionResult Owasp03LandingPage(string id)
        {
            HttpCookie cookie = new HttpCookie("ASP.NET_SessionId") { Value = id };
            ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            var str = User.Identity.IsAuthenticated ?
                "Logged in as " + User.Identity.Name :
                "Not Logged in";

            str += Session["MySecret"];
            return View("/Owasp/03ProblemAuth/OWasp03Secret", str);
        }
    }
}
