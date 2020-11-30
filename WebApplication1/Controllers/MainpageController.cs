using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class MainpageController : Controller
    {
        // GET: Mainpage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["employee"] = null;
            return RedirectToAction("index", "Employees");
        }

        public ActionResult Customers_list()
        {
            //if (Session["employee"] != null)
            //{
                return RedirectToAction("customerslist", "Customers");
            //}
            //else
            //{
            //    return RedirectToAction("index", "Mainpage", new { error = 2 });
            //}
        }
        public ActionResult MainPage()
        {

            return RedirectToAction("index", "Mainpage");
        }
        public ActionResult Charging_requests()
        {
            if (Session["employee"] != null)
            {
                return RedirectToAction("chargingrequests", "Topups");
            }
            else
            {
                return RedirectToAction("index", "Mainpage", new { error = 2 });
            }

        }

        public ActionResult Transfer_requests()
        {
            if (Session["employee"] != null)
            {
                return RedirectToAction("transferrequests", "Transfer");
            }
            else
            {
                return RedirectToAction("index", "Mainpage", new { error = 2 });
            }

        }


    }
}