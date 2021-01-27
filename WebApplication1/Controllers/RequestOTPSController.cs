using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RequestOTPSController : Controller
    {
        // GET: RequestOTPS
        public ActionResult Index()
        {

            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            return View();
        }
        public ActionResult RequestOTP()
        {

            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                return View();
            }
        }
        public ActionResult GetOTP(string MeterId, int Amount,string SerialNUM)
        {

            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                int rc;
                CashCard[] cashCard = CashCard.GetCashCards(new CashCardParameters { SerialNumber = SerialNUM }, out rc);
                Topup topup = new Topup(MeterId, Amount, cashCard[0].SerialNumber);
                topup.SaveData();
                return RedirectToAction("ShowOTP", "RequestOTPS", new { otp = topup.OTP });
            }
        }

        public ActionResult ShowOTP(string otp,string success)
        {

            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                if (otp != null && success == null)
                {
                    ViewBag.otp = otp;

                }
                else if (success != null)
                {
                    ViewBag.success = success;
                }

                return View();
            }
        
        }
        public ActionResult Charge_this_otp(int? otp)
        {

            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                int rc;
                Topup[] topups = Topup.GetTopups(new TopupParameters { OTP = otp }, out rc);
                topups[0].Charged();


                return RedirectToAction("ShowOTP", "RequestOTPS", new { success = "charged"});
            }
        }
    }
}