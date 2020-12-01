using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ReturnOTPController : Controller
    {
        // GET: ReturnOTP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Return( int meterid)
        {
            if (Session["customer"] != null)
            {
                int rc;
                Customer customer = (Session["customer"] as Customer);
                Meter[] meter = Meter.GetMeters(new MeterParameters { Meterid = meterid }, out rc);
                if (customer.Id == meter[0].UserId)
                {
                    return RedirectToAction("OTPS", "ReturnOTP", new { meterid = meterid });

                }

                else if (customer.Id == meter[0].UserId)
                {
                    string response;
                    Customer customer1 = new Customer(meter[0].UserId);
                    using (WebClient client = new WebClient())
                    {
                        int telephone = Convert.ToInt32("97" + customer1.Telephone);
                        response = client.DownloadString($"http://sms.htd.ps/API/SendSMS.aspx?id=eadaaac72e504a1f6e0b2a7a5cb60dc9&sender=easycharge1&to=telephone&msg=welcometoeasychargesomeonetrytoreturnyourotp&mode=1");
                        //"OK|970123456789:serial"
                        //sms.Id=

                        string[] ss = response.Split((new char[] { '|' }));
                        string[] sss = ss[1].Split((new char[] { ':' }));
                        if (ss[0] == "OK")
                        {
                            return RedirectToAction("OTPS", "ReturnOTP", new { meterid = meterid });
                        }
                        else
                        {
                            return RedirectToAction("index", "ReturnOTP");
                        }
                    }
                }
                return View();
            }
            else
            {
                return RedirectToAction("index", "ReturnOTP");
            }
        }

        public ActionResult OTPS(int meterid) {
            ViewBag.meterid = meterid;
            return View();
        }
    }
}