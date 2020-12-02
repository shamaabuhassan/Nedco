﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TopupsController : Controller
    {
        // GET: Topups
        public ActionResult Index(int? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Search(int? MeterId)
        {
            int rc;
            if (MeterId != null)
            {
                Topup[] topups = Topup.GetTopups(new TopupParameters { MeterId = MeterId }, out rc);
                ViewBag.topups = topups;
            }
            return View();

        }



        public ActionResult Save(int? id, int? meterId, decimal? amount, int? cardId)
        {
            int rc;
            Meter[] meter = Meter.GetMeters(new MeterParameters { Meterid = meterId }, out rc);//user of meter
            Customer customer = (Session["customer"] as Customer);
            CashCard cashCard = new CashCard(cardId);
            Customer customer1 = new Customer(meter[0].UserId);
            if (Session["customer"] != null)
            {
                if (customer.Id == meter[0].UserId && customer.CardId == cashCard.Cardid)//for himself from his card
                {
                    SMS sms = new SMS();
                    sms.To_number = customer.Telephone;
                    sms.Msg = $"أهلا وسهلا بك أنت تقوم بشحن {amount} الى عدادك الان";
                    string status = sms.Send();
                    if (status == "OK")
                    {
                        Topup topup = new Topup(id, meterId, amount, cardId);
                        int result;
                        result = topup.SaveData();
                        ViewBag.result = result;
                    }
                    else
                    {
                        return RedirectToAction("Save", "Tpoups");
                    }
                }

                else if (customer.Id == meter[0].UserId && customer.CardId != cashCard.Cardid) //for himself from another card
                {

                    Customer[] customer2 = Customer.GetCustomers(new CustomerParameters { CardId = cardId }, out rc);
                    SMS sms = new SMS();
                    sms.To_number = customer2[0].Telephone;
                    sms.Msg = $" يحاول {customer.name} شحن عداده باستخدام البطاقة الخاصة بك بقيمة {amount}";
                    string status = sms.Send();
                    if (status == "OK")
                    {
                        Topup topup = new Topup(id, meterId, amount, cardId);
                        int result;
                        result = topup.SaveData();
                        ViewBag.result = result;
                    }
                    else
                    {
                        return RedirectToAction("Save", "Topups");
                    }

                }

                else if (customer.Id != meter[0].UserId && customer.CardId == cashCard.Cardid)//for another from his card
                {
                    SMS sms = new SMS();
                    sms.To_number = customer1.Telephone;
                    sms.Msg = $"يحاول {customer.name} شحن عدادك بقيمة {amount}";
                    string status = sms.Send();

                    SMS sms1 = new SMS();
                    sms1.To_number = customer.Telephone;
                    sms1.Msg = $"يحاول {customer1.name} شحن عداده باستخدام بطاقتك بقيمة {amount}";
                    string status1 = sms1.Send();

                    if (status == "OK" && status1 == "OK")
                    {

                        Topup topup = new Topup(id, meterId, amount, cardId);
                        int result;
                        result = topup.SaveData();
                        ViewBag.result = result;
                    }
                    else
                    {
                        return RedirectToAction("Save", "Topups");
                    }
                }


                else if (customer.Id != meter[0].UserId && customer.CardId != cashCard.Cardid && customer1.CardId != cashCard.Cardid)//for another from another card
                {
                    int rrc;
                    Customer[] customer2 = Customer.GetCustomers(new CustomerParameters { CardId = cardId }, out rrc);

                    SMS sms = new SMS();
                    sms.To_number = customer2[0].Telephone;
                    sms.Msg = $"يحاول {customer1.name} شحن عداده بقيمة {amount} باستخدام بطاقتك";
                    string status = sms.Send();

                    SMS sms1 = new SMS();
                    sms1.To_number = customer1.Telephone;
                    sms1.Msg = $"يحاول {customer.name} شحن عدادك بقيمة {amount} باستخدام بطاقة {customer2[0].name}";
                    string status1 = sms1.Send();
                    if (status == "OK" && status1 == "OK")
                    {
                        Topup topup = new Topup(id, meterId, amount, cardId);
                        int result;
                        result = topup.SaveData();
                        ViewBag.result = result;
                    }
                    else
                    {
                        return RedirectToAction("Save", "Topups");
                    }

                }
                else if (customer.Id != meter[0].UserId && customer.CardId != cashCard.Cardid && customer1.CardId == cashCard.Cardid)//for another from the another card
                {

                    SMS sms = new SMS();
                    sms.To_number = customer1.Telephone;
                    sms.Msg = $"يحاول {customer.name} شحن عدادك باستخدام بطاقتك";
                    string status = sms.Send();

                    SMS sms1 = new SMS();
                    sms1.To_number = customer.Telephone;
                    sms1.Msg = $"أهلا وسهلا بك أنت تحاول الان شحن عداد {customer1.name} باستخدام بطاقته {customer1.CardId}";
                    string status1 = sms1.Send();


                    if (status == "OK" && status1 == "OK")
                    {
                        Topup topup = new Topup(id, meterId, amount, cardId);
                        int result;
                        result = topup.SaveData();
                        ViewBag.result = result;
                    }
                    else
                    {
                        return RedirectToAction("Save", "Topups");
                    }
                }
                return View();
            }
            else
            {
                return RedirectToAction("Save", "Topups");
            }


        }


        public ActionResult chargingrequests()
        {
            if (Session["employee"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index", "Mainpage", new { error = 2 });
            }
        }

        public ActionResult Charged()
        {
            if (Session["employee"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index", "MainPage");
            }
        }

        public ActionResult ChargeMeter(int id)
        {
            Topup topup = new Topup(id);
            topup.Charged();
            return RedirectToAction("Charged", "Topups");

        }

        public ActionResult ChargeHist()
        {
            return View();


        }

        public ActionResult Monthlycharging(string month, string year)
        {
            int rc;
            Customer customer = (Session["customer"] as Customer);
            Meter[] meters = Meter.GetMeters(new MeterParameters { UserId = customer.Id }, out rc);

            if (Session["customer"] != null)
            {

                Topup[] topups = Topup.GetTopups(new TopupParameters { Month = month, Year = year, MeterId = meters[0].Meterid }, out rc);

                decimal? amount = 0;
                decimal? count = 0;
                foreach (Topup topup in topups)
                {
                    amount += topup.Amount;
                    count += 1;
                }
                ViewBag.amount = amount;
                ViewBag.count = count;

            }
            return View();
        }
    }
}