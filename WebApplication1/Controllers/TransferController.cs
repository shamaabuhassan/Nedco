using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TransferController : Controller
    {
        // GET: Transfer
        public ActionResult Index(int ?id)
        {
            ViewBag.id = id;
            return View();
        }
        
        public ActionResult Search(int? MeterId)
        {
            int rc;
            if (MeterId != null)
            {
                Transfer[] transfers = Transfer.GetTransfers(new TransferParameters { MeterId = MeterId }, out rc);
                ViewBag.transfers = transfers;
            }
            return View();
        }

        public ActionResult Save(int? id, string senderOTP, int? meterId, decimal? amount)
        {
            if (senderOTP != null)
            {
                Transfer transfer = new Transfer(id, senderOTP, meterId, amount);
                int result;
                result = transfer.SaveData();
                ViewBag.result = result;
            }
            return View();
        }

        public ActionResult transferrequests()
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
    }
}