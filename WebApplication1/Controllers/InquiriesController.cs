using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public ActionResult Charges(int? MeterId)
        {
            ViewBag.MeterId = MeterId;
            return View();
        }

        public ActionResult Transfers(int? MeterId)
        {
            int rc;
            if (MeterId != null)
            {
                Transfer[] transfers = Transfer.GetTransfers(new TransferParameters { MeterId = MeterId }, out rc);
                Transfer[] transfers2 = Transfer.GetTransfersBySenderOTP(new TransferParameters { MeterId = MeterId }, out rc);
                if (transfers != null && transfers2==null)
                {
                    ViewBag.transfers = transfers;
                    ViewBag.MeterId = MeterId;
                    return View();
                }
                else if(transfers == null && transfers2 != null)
                {
                    return RedirectToAction("Transfrom", "Transfers", new {transfers2=transfers2});
                }
                else if (transfers != null && transfers2 != null)
                {
                    return RedirectToAction("Trans_from_to", "Transfers", new { transfers2 = transfers2,transfers=transfers });
                }
            }
            
                return View();
            
            
        }

        public ActionResult MonthlyCharge(DateTime ?fromdate,DateTime ?todate,int ?MeterId)
        {

            int rc;
            Topup[] topups = Topup.GetMonthlyTopups(new TopupParameters { fromdate =fromdate, todate=todate,MeterId= MeterId }, out rc);
           
            decimal? amount = 0;
            decimal? count = 0;
            foreach(Topup topup in topups)
            {
                amount += topup.Amount;
                count += 1;
            }
            ViewBag.amount = amount;
            ViewBag.count = count;
            return View();
        }

    


    }
}