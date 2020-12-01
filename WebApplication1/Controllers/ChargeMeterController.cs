using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ChargeMeterController : Controller
    {
        // GET: ChargeMeter
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Charge(string otp)
        {
            if (Session["customer"] != null)
            {
                Customer customer = (Session["customer"] as Customer);
                int rc;
                Topup[] topup = Topup.GetTopups(new TopupParameters { OTP = otp }, out rc);

                Meter[] meters = Meter.GetMeters(new MeterParameters { Meterid = topup[0].MeterId }, out rc);

                if (customer.Id == meters[0].UserId)
                {
                    topup[0].Charged();
                }

                else if (customer.Id != meters[0].UserId)
                {
                    string response;
                    Customer customer1 = new Customer(meters[0].UserId);
                    using (WebClient client = new WebClient())
                    {
                        string telephone = $"97{customer.Telephone}";
                        string message = $" يحاول {customer.name} تحويل {topup[0].Amount}شيكل لحسابك رقم{customer1.CardId} ";
                        response = client.DownloadString($"http://sms.htd.ps/API/SendSMS.aspx?id=eadaaac72e504a1f6e0b2a7a5cb60dc9&sender=easycharge1&to={telephone}&msg=welcometoeasychargesomeonetrytochargeyourmeter&mode=1");
                        //"OK|970123456789:serial"
                        //sms.Id=

                        string[] ss = response.Split((new char[] { '|' }));
                        string[] sss = ss[1].Split((new char[] { ':' }));
                        if (ss[0] == "OK")
                        {
                            topup[0].Charged();
                        }
                        else
                        {
                            return RedirectToAction("index", "ChargeMeter");
                        }
                    }

                }
                return View();
            }
            else
            {
                return RedirectToAction("index", "ChargeMeter");
            }
        }
    }
}