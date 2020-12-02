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

        public ActionResult Charges(int meter)
        {
            ViewBag.meter = meter;
            return View();
        }

        public ActionResult Transfers(int meter)
        {
            ViewBag.meter = meter;
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

       public ActionResult Transferom(int meter,Transfer[] transfer)
        {
            int rc;
            int count = 0;
            string[] otps = null;
            Topup[] topups = Topup.GetTopups(new TopupParameters { }, out rc);//for topups
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