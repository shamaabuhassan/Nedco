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
            Meter meter = new Meter(meterId);
            Customer customer = (Session["customer"] as Customer);
            CashCard cashCard = new CashCard(cardId);

            if(customer.Id==meter.UserId && customer.CardId==cashCard.Cardid )
            {
                Topup topup = new Topup(id, meterId, amount, cardId);
                int result;
                result = topup.SaveData();
                ViewBag.result = result;
            }
            else
            {
                Customer customer1 = new Customer(meter.UserId);
               
                string response;
                using (WebClient client = new WebClient())
                {
                    int telephone = Convert.ToInt32("97" + customer1.Telephone);
                    response = client.DownloadString($"http://sms.htd.ps/API/SendSMS.aspx?id=eadaaac72e504a1f6e0b2a7a5cb60dc9&sender=easycharge1&to=telephone&msg=welcometoeasychargesomeonetrytochargeyourmeter&mode=1");
                    //"OK|970123456789:serial"
                    //sms.Id=
                    
                        string[] ss = response.Split((new char[] { '|' }));
                        string[] sss = ss[1].Split((new char[] { ':' }));
                    if (ss[0] == "OK") {
                        SMS sms = new SMS();
                        sms.Status = ss[0];
                        sms.To_number = Convert.ToInt32( sss[0]);
                        sms.SMS_Id= Convert.ToInt32(sss[1]);
                        sms.SaveData();
                        Topup topup = new Topup(id, meterId, amount, cardId);
                        int result;
                        result = topup.SaveData();
                        ViewBag.result = result;
                    }
                    
                }
            }
            return View();
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