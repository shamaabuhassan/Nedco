using System;
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
            Meter []meter = Meter.GetMeters(new MeterParameters{ Meterid=meterId},out rc);//user of meter
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
                   string status= sms.Send();
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
                    string status=sms.Send();
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
                   string status= sms.Send();

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
                        if (status== "OK" && status1 == "OK")
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
                    string response;

                    using (WebClient client = new WebClient())
                    {
                        int telephone = Convert.ToInt32("97" + customer1.Telephone);
                        response = client.DownloadString($"http://sms.htd.ps/API/SendSMS.aspx?id=eadaaac72e504a1f6e0b2a7a5cb60dc9&sender=easycharge1&to=telephone&msg=welcometoeasychargesomeonetrytochargeyourmeter&mode=1");
                        //"OK|970123456789:serial"
                        //sms.Id=

                        string[] ss = response.Split((new char[] { '|' }));
                        string[] sss = ss[1].Split((new char[] { ':' }));
                        if (ss[0] == "OK")
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
    }
}