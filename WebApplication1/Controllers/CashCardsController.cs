using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CashCardsController : Controller
    {
        // GET: CashCards
       public ActionResult Index(int ?ID)
        {
            ViewBag.ID = ID;
            return View();
        }
        

        public ActionResult Search(int? Cardid)
        {
            int rc;
            if (Cardid != null)
            {
                CashCard[] cashCards = CashCard.GetCashCards(new CashCardParameters { Cardid = Cardid }, out rc);
                ViewBag.cashCards = cashCards;
            }
            return View();
        }

        public ActionResult Save(int? Id,string Password, int? CustomerId, decimal? Amount,int ?Cardid)
        {
            if (Password != null)
            {
                CashCard cashCard = new CashCard(Id, Password, Amount,Cardid);
                int result;
                result = cashCard.SaveData();
                ViewBag.result = result;
            }
            return View();
        }

        /*public ActionResult GetMetersXHR(MeterParameters parameters)
        {
            int rowsCount;
            Meter[] meters = Meter.GetMeters(parameters, out rowsCount);
            return Content(JsonConvert.SerializeObject(meters));
        }
        /*
         {
         Id: "5",
         CustomerId: 120,

         }
         [
         {Id: 88, Customer: 00},
         {Id: --, ...}
         ]
         
         */
    }
}