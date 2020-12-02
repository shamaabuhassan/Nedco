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

        public ActionResult NewCard(int? Id, string Password, int? CustomerId, decimal? Amount, int? Cardid)
        {

            if (Password != null)
            {
                CashCard cashCard = new CashCard(Id, Password, Amount, Cardid);
                int result;
                result = cashCard.SaveData();
                ViewBag.result = result;
            }
            return View();

        }

        public ActionResult Newcustomer(int? id, string username, int? cardId, string telephone, int? countryId, int? cityId, string town, string street, string password, string name)
        {
            if (username != null)
            {
                Customer customer = new Customer(id, username, cardId, telephone, countryId, cityId, town, street, password, name);
                int result;
                result = customer.SaveData();
                ViewBag.result = result;
            }
            return View();
        }

        public ActionResult NewMeter(int? id, int? userId, decimal? amount, int? meterid)
        {
            if (userId != null)
            {
                Meter meter = new Meter(id, userId, amount, meterid);
                int result;
                result = meter.SaveData();
                ViewBag.result = result;
            }
            return View();
        }
    }
}