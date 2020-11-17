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
            if (MeterId != null)
            {
                Topup[] topups = Topup.GetTopups(new TopupParameters { MeterId = MeterId }, out rc);
                ViewBag.topups = topups;
            }
            return View();

        }

        public ActionResult Save(int? id, int? meterId, decimal? amount, int? cardId)
        {
            if (meterId != null)
            {
                Topup topup = new Topup(id, meterId, amount, cardId);
                int result;
                result = topup.SaveData();
                ViewBag.result = result;
            }
            return View();
        }
    }
}