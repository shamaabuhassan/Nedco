using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            int rc;
            Topup[] topup = Topup.GetTopups(new TopupParameters {OTP =senderOTP }, out rc);//get senderotp meterid 
            Meter[] meters = Meter.GetMeters(new MeterParameters {Meterid=topup[0].MeterId}, out rc);//get meterid customer
            Customer customer = (Session["customer"] as Customer);//check if senderotp customer as session customer
            Meter[] meters1 = Meter.GetMeters(new MeterParameters { Meterid =meterId }, out rc);// get userid will take the amount
            Customer customer1 = new Customer(meters1[0].UserId);//get customer info


            if (Session["customer"] != null)
            {
                if (customer.Id == meters[0].UserId) {

                    SMS sms = new SMS();
                    sms.To_number = customer.Telephone;
                    sms.Msg = $"أهلا وسلا بك في تطبيقنا أنت تحاول الان تحويل قيمة {amount} الي حساب {customer1.name} ";
                    string status = sms.Send();

                    SMS sms1 = new SMS();
                    sms.To_number = customer1.Telephone;
                    sms.Msg = $"يحاول {customer.name} تحويل قيمة {amount} الى عدادك";
                    string status1 = sms1.Send();
                    if (status == "OK" && status1 == "OK")
                    {
                        Transfer transfer = new Transfer(id, senderOTP, meterId, amount);
                        int result;
                        result = transfer.SaveData();
                        ViewBag.result = result;

                    }
                }
             
                else if(customer.Id != meters[0].UserId)
                {
                   Customer customer2=new Customer (meters[0].UserId);
                    SMS sms = new SMS();
                    sms.To_number = customer2.Telephone;
                    sms.Msg = $"أهلا وسلا بك في تطبيقنا أنت تحاول الان تحويل قيمة {amount} الي حساب {customer1.name} ";
                    string status = sms.Send();

                    SMS sms1 = new SMS();
                    sms.To_number = customer1.Telephone;
                    sms.Msg = $"يحاول {customer2.name} تحويل قيمة {amount} الى عدادك";
                    string status1 = sms1.Send();
                    if (status == "OK" && status1 == "OK")
                    {
                        Transfer transfer = new Transfer(id, senderOTP, meterId, amount);
                        int result;
                        result = transfer.SaveData();
                        ViewBag.result = result;

                    }

                }
                return View();
            }
            else
            {
                return RedirectToAction("Save", "Transfer");
            }

            
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

        public ActionResult Transformfrom(int meter)
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
                    if (transfer1.SenderOTP == topup1.OTP)
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
                foreach (string otp in otps)
                {
                    foreach (Transfer transfer1 in transfer)
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