using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TopupsController : Controller
    {
        // GET: Topups
        public ActionResult Index(int ?id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Search(int ?MeterId)
        {
            int rc;
            Topup[] topups = Topup.GetTopups(new TopupParameters { MeterId = MeterId }, out rc);
            ViewBag.topups = topups;
            return View();

        }

        public ActionResult Save(int? id, int? meterId, decimal? amount, int? cardId, string otp, DateTime? chargeDate, DateTime? activationDate, string status)
        {
            Topup topup = new Topup(id, meterId, amount, cardId, otp, chargeDate, activationDate, status);
            int result;
            result = topup.SaveData();
            ViewBag.result = result;
            return View();
        }
    }
}