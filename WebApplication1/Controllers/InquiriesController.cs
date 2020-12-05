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
            ViewBag.MeterId = MeterId;
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

       public ActionResult Transfrom(int meter)
        {
            int rc;
            int count = 0;
            string[] otps = null;
            Topup[] topups = Topup.GetTopups(new TopupParameters { }, out rc);//for topups
            Transfer[] transfer = Transfer.GetTransfers(new TransferParameters { }, out rc);//all trans

            Topup[] topups1 = null;//for senders otp

            foreach (Transfer transfer1 in transfer)
            {
                foreach (Topup topup1 in topups)
                {
                    if(transfer1.SenderOTP==topup1.OTP)
                    {
                        topups1[count] = topup1;
                        count += 1;
                    }
                }

            }
            int count1 = 0;
            int count2 = 0;//for otps
            foreach (Topup topup in topups1)
            {
               
                if (topups1[count1].MeterId == meter)
                {
                    otps[count2] = topups1[0].OTP;
                    count2 += 1;
                }
                count1 += 1;
            }

            Transfer[] transfers = null;// for senders
            int count3 = 0;
            if (otps != null)
            {
             foreach( string otp  in otps)
                {
                    foreach(Transfer transfer1 in transfer)
                    {
                        if (otp == transfer1.SenderOTP)
                        {
                            transfers[count3] = transfer1;
                            count3 += 1;
                        }
                    }
                }
            }

            ViewBag.transfers = transfers;
            return View();
        }


    }
}