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
            return View();
        }
        public ActionResult RequestOTP()
        {
            return View();
        }
        public ActionResult GetOTP(int ? MeterId, int Amount,int CardId)
        {
            int rc;
            CashCard[] cashCard = CashCard.GetCashCards(new CashCardParameters {SerialNumber = CardId }, out rc);
            Topup topup = new Topup(MeterId, Amount, cashCard[0].Id);
            topup.SaveData();
            return View();
        }
    }
}