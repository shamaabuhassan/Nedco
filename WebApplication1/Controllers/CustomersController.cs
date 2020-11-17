using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index(string name)
        {
            ViewBag.name = name;
            return View();
        }

        public ActionResult Search(int? CountryId)
        {
            int rc;
            if (CountryId != null)
            {
                Customer[] customers = Customer.GetCustomers(new CustomerParameters { CountryId = CountryId }, out rc);
                ViewBag.customers = customers;
            }
            return View();
        }

        public ActionResult Save(int? id, string username, int? meterId, int? cardId, string telephone, int? countryId, int? cityId, string area, string street, string password,string name)
        {
            if (meterId != null) { 
            Customer customer = new Customer(id, username, meterId, cardId, telephone, countryId, cityId, area, street, password, name);
            int result;
            result = customer.SaveData();
            ViewBag.result = result;
        }
            return View();
        }
    }
}