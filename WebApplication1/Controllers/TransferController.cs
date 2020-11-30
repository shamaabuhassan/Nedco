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
            Customer customer = (Session["customer"] as Customer);
            Meter meter = new Meter(meterId);
            Customer customer1 = new Customer(meter.Id);

           
                string response;
                string response1;
                using (WebClient client = new WebClient())
                {
                    int telephone = Convert.ToInt32("97" + customer.Telephone);
                    int telephone1 = Convert.ToInt32("97" + customer1.Telephone);

                    response = client.DownloadString($"http://sms.htd.ps/API/SendSMS.aspx?id=eadaaac72e504a1f6e0b2a7a5cb60dc9&sender=easycharge1&to=telephone&msg=welcometoeasychargesomeonetrytotransferfromyourmeter&mode=1");
                    response1 = client.DownloadString($"http://sms.htd.ps/API/SendSMS.aspx?id=eadaaac72e504a1f6e0b2a7a5cb60dc9&sender=easycharge1&to=telephone1&msg=welcometoeasychargesomeonetrytosendanotpforyourmeter&mode=1");
                    //"OK|970123456789:serial"
                    //sms.Id=

                    string[] ss = response.Split((new char[] { '|' }));
                    string[] sss = ss[1].Split((new char[] { ':' }));
                    if (ss[0] == "OK")
                    {
                        SMS sms = new SMS();
                        sms.Status = ss[0];
                        sms.To_number = Convert.ToInt32(sss[0]);
                        sms.SMS_Id = Convert.ToInt32(sss[1]);
                        sms.SaveData();

                        Transfer transfer = new Transfer(id, senderOTP, meterId, amount);
                        int result;
                        result = transfer.SaveData();
                        ViewBag.result = result;
                    }
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