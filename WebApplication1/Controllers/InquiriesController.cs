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
            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            return View();
        }

        public ActionResult Charges(string MeterId)
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                ViewBag.MeterId = MeterId;
                return View();
            }
        }

        public ActionResult Transfers(string MeterId)
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                int rc;
                if (MeterId != null)
                {
                    Transfer[] transfers = Transfer.GetTransfers(new TransferParameters { MeterId = MeterId }, out rc);

                    Transfer[] transfers2 = Transfer.GetTransfersBySenderOTP(new TransferParameters { MeterId = MeterId }, out rc); //get meter of senderotp

                    if (transfers.Length != 0 && transfers2.Length == 0)
                    {
                        ViewBag.transfers = transfers;
                        ViewBag.MeterId = MeterId;
                        return View();
                    }
                    else if (transfers.Length == 0 && transfers2.Length != 0)
                    {
                        return RedirectToAction("Transfrom", "Transfer", new { transfers2 = transfers2 });
                    }
                    else if (transfers.Length != 0 && transfers2.Length != 0)
                    {
                        
                        return RedirectToAction("Trans_from_to", "Transfer", new { transfers2 = transfers2, transfers = transfers });
                    }
                }
            }

            return View();
        }

        public ActionResult MonthlyCharge(DateTime? fromdate, DateTime? todate, string MeterId)
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            else
            {
                int rc;
                Topup[] topups = Topup.GetMonthlyTopups(new TopupParameters { fromdate = fromdate, todate = todate, MeterId = MeterId }, out rc);

                decimal? amount = 0;
                decimal? count = 0;
                foreach (Topup topup in topups)
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
}