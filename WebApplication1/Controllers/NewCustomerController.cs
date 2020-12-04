using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class NewCustomerController : Controller
    {
        // GET: NewCustomer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewCard( string Password, int? CustomerId, decimal? Amount, int? Cardid)
        {

            if (Password != null)
            {
                CashCard cashCard = new CashCard(null, Password, Amount, Cardid);
                int result;
                result = cashCard.SaveData();
                ViewBag.result = result;
            }
            return View();

        }

        public ActionResult Newcustomer(string username, int? cardId, string telephone, int? countryId, int? cityId, string town, string street, string password, string name)
        {
            if (username != null)
            {
                Customer customer = new Customer(null, username, cardId, telephone, countryId, cityId, town, street, password, name);
                int result;
                result = customer.SaveData();
                ViewBag.result = result;
            }
            return View();
        }

        public ActionResult NewMeter( int? userId, decimal? amount, int? meterid)
        {
            if (userId != null)
            {
                Meter meter = new Meter(null, userId, amount, meterid);
                int result;
                result = meter.SaveData();
                ViewBag.result = result;
            }
            return View();
        }
    }
}