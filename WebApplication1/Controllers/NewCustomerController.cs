﻿using System;
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

        public ActionResult NewCard(decimal? Amount, string SerialNumber, int? rresult)
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                if (rresult == 1)
                {
                    ViewBag.result = rresult;
                    return View();

                }
                CashCard cashCard = new CashCard(null, Amount, SerialNumber);
                //   int result;
                //result = cashCard.SaveData();
               
                return View(cashCard);

             
            }
        }

        [HttpPost]
        public ActionResult NewCard(CashCard cashCard)
        {
            int? result = 0;

            
            if (ModelState.IsValid)
            { //checking model state

                //check whether id is already exists in the database or not
              
                result = cashCard.SaveData();

                if (result == 0)
                {
                    ModelState.AddModelError("SerialNumber", "serial number  is less or more than 12 digits");
                    ViewBag.result = result;
                    return View(cashCard);
                }
                else if (result == 2)
                {
                    ModelState.AddModelError("SerialNumber", "serial number is exist");
                    ViewBag.result = result;
                    return View(cashCard);
                }
                else
                {
                    //ModelState.Clear();
                    // ViewBag.result = result;
                    return RedirectToAction("NewCard", "NewCustomer", new { rresult = result });
                }
            }

            else
            {
                ViewBag.result = result;
                return View(cashCard);
            }
            
        }

        public ActionResult Newcustomer(string username, int? cardId, string telephone, int? countryId, int? cityId, string town, string street, string password, string name, int? rresult)
        {

            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                if (rresult == 1)
                {
                    ViewBag.result = rresult;
                    return View();

                }

                Customer customer = new Customer(null, username, cardId, telephone, countryId, cityId, town, street, password, name);

                return View(customer);
            }
            
        }



        [HttpPost]
        public ActionResult Newcustomer(Customer customer)
        {
            
            int? result = 0;
           
            if (ModelState.IsValid)
            { //checking model state

                //check whether id is already exists in the database or not

                result = customer.SaveData();

                if (result == 0)
                {
                    ModelState.AddModelError("Telephone", " telephone number must contains 10 numbers");
                    ViewBag.result = result;
                    return View(customer);
                }
                else if (result == 2)
                {
                    ModelState.AddModelError("Username", "Username is exist");
                    ViewBag.result = result;
                    return View(customer);
                }
                else if (result == 3)
                {
                    ModelState.AddModelError("Telephone", "the telphone number must start with either 056 or 059 ");
                    ViewBag.result = result;
                    return View(customer);
                }
                else
                {

                    return RedirectToAction("Newcustomer", "NewCustomer", new { rresult = result });
                    //ViewBag.result = result;
                    //return View(customer);

                }
            }

            else
            {
                ViewBag.result = result;
                return View(customer);
            }

        }


        public ActionResult NewMeter( int? userId, decimal? amount, string meterid, int? rresult)
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                if (rresult == 1)
                {
                    ViewBag.result = rresult;
                    return View();

                }
                Meter meter = new Meter(userId, amount, meterid);
                return View(meter);
            }
        }



        [HttpPost]
        public ActionResult NewMeter(Meter meter)
        {
            int? result = 0;
           
            if (ModelState.IsValid)
            { //checking model state

                //check whether id is already exists in the database or not

                result = meter.SaveData();

                if (result == 0)
                {
                    ModelState.AddModelError("Meterid", "meter id is less or more than 12 digits");
                    ViewBag.result = result;
                    return View(meter);
                }
                else if (result == 2)
                {
                    ModelState.AddModelError("Meterid", "meter id is exist");
                    ViewBag.result = result;
                    return View(meter);
                }
                else
                {

                    //ViewBag.result = result;
                    //return View(meter);
                    return RedirectToAction("NewMeter", "NewCustomer", new { rresult = result });
                }
            }

            else
            {
                ViewBag.result = result;
                return View(meter);
            }

        }

    }
}