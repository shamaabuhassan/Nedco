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
            Customer customer = (Session["customer"] as Customer);
            int rc;
            Topup[] topup = Topup.GetTopups(new TopupParameters { OTP = otp }, out rc);
            Meter[] meters = Meter.GetMeters(new MeterParameters { Meterid = topup[0].MeterId }, out rc);
            if (Session["customer"] != null)
            {
                if (customer.Id == meters[0].UserId)
                {
                    SMS sms = new SMS();
                    sms.To_number = customer.Telephone;
                    sms.Msg = $"  أهلا وسهلا بك أنت تحاول الان شحن عدادك باستخدام موقعنا في الشركة ورقم الكود الذي يريد شحنه هو {otp}";
                    string status = sms.Send();
                    if (status == "OK")
                    {
                        topup[0].Charged();
                    }
                    else
                    {
                        return RedirectToAction("index", "ChargeMeter");
                    }
                }

                else if (customer.Id != meters[0].UserId)
                {
                    Customer customer1 = new Customer(meters[0].UserId);
                    SMS sms = new SMS();
                    sms.To_number = customer1.Telephone;
                    sms.Msg = $"يحاول {customer.name} شحن عدادك باستخدام موقنا في الشركة ورقم الكود الذي يريد شحنه هو {otp}";
                    string status = sms.Send();
                    if (status == "OK")
                    {
                        topup[0].Charged();
                    }
                    else
                    {
                        return RedirectToAction("index", "ChargeMeter");
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