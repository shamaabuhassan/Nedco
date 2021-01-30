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
            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            return View();
        }

        public ActionResult NewCard(decimal? Amount, string SerialNumber)
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                CashCard cashCard = new CashCard(null, Amount, SerialNumber);
             //   int result;
                //result = cashCard.SaveData();
                return View(cashCard);
            }
        }

        [HttpPost]
        public ActionResult NewCard(CashCard cashCard)
        {
                if (ModelState.IsValid)
                { //checking model state

                    //check whether id is already exists in the database or not
                    int result;
                    result = cashCard.SaveData();

                    if (result == 0)
                    {
                        ModelState.AddModelError("card id", "card id is less than 12 digits");
                    return View(cashCard);
                    }
                return RedirectToAction("NewCard");
                }
                return View(cashCard);
            
        }

        public ActionResult Newcustomer(string username, int? cardId, string telephone, int? countryId, int? cityId, string town, string street, string password, string name)
        {

            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                if (username != null)
                {
                    Customer customer = new Customer(null, username, cardId, telephone, countryId, cityId, town, street, password, name);
                    int result;
                    result = customer.SaveData();
                    ViewBag.result = result;
                }
            }
            return View();
        }

        public ActionResult NewMeter( int? userId, decimal? amount, string meterid)
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                if (userId != null)
                {
                    Meter meter = new Meter(userId, amount, meterid);
                    int result;
                    result = meter.SaveData();
                    ViewBag.result = result;
                }
                return View();
            }
        }
    }
}