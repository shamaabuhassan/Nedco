using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class AreaController : Controller
    {
        // GET: Area
        public ActionResult Index()
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            return View();
        }
    }
}