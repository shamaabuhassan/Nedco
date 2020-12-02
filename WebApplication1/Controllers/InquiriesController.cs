using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class InquiriesController : Controller
    {
        // GET: Inquiries
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Charges()
        {
            return View();
        }

        public ActionResult Transfers()
        {
            return View();
        }

        public ActionResult MonthlyCharge(string month,string year,int meter)
        {

            int rc;
            Topup[] topups = Topup.GetTopups(new TopupParameters { Month =month, Year =year,MeterId=meter}, out rc);
           
            decimal? amount = 0;
            foreach(Topup topup in topups)
            {
                amount += topup.Amount;
            }
            ViewBag.amount = amount;
            return View();
        }


    }
}