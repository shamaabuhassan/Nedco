using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class apiController : Controller
    {
        // GET: api
        public ActionResult CheckLogin(string username, string password)
        {
            Customer customer = Customer.CheckLogin(username, password);
            if (customer != null)
            {
                return Content(JsonConvert.SerializeObject(new { result = "success", data = customer }));
            }
            else
            {
                return Content(JsonConvert.SerializeObject(new { result = "error", data = "the username or password is wrong" }));
            }

        }

        public ActionResult RequestOTP(int? meterId, int? amount, int SerialNumber, int customerid)
        //this should reach the website button which request OTP 

        {
            int rc;
            Customer customer = new Customer(customerid);
            Meter[] meter = Meter.GetMeters(new MeterParameters { Meterid = meterId }, out rc);//user of meter
            CashCard[] cashCards = CashCard.GetCashCards(new CashCardParameters { SerialNumber = SerialNumber }, out rc);
            Customer customer1 = new Customer(meter[0].UserId);

            if (customer.Id == meter[0].UserId && customer.CardId == cashCards[0].Id)//for himself from his card
            {
                SMS sms = new SMS();
                sms.To_number = customer.Telephone;
                sms.Msg = $"أهلا وسهلا بك أنت تقوم بشحن {amount} الى عدادك الان";
                // string status = sms.Send();
                sms.SaveData();
                //if (status == "OK")
                {
                    Topup topup = new Topup(null, meterId, amount, cashCards[0].Id);
                    
                    int result;
                    result = topup.SaveData();
                    return Content(JsonConvert.SerializeObject(new { result = "success", data = topup }));
                }
                
            }

            else if (customer.Id == meter[0].UserId && customer.CardId != cashCards[0].Id) //for himself from another card
            {

                Customer[] customer2 = Customer.GetCustomers(new CustomerParameters { CardId = cashCards[0].Id }, out rc);
                SMS sms = new SMS();
                sms.To_number = customer2[0].Telephone;
                sms.Msg = $" يحاول {customer.Name} شحن عداده باستخدام البطاقة الخاصة بك بقيمة {amount}";
                //  string status = sms.Send();
                sms.SaveData();
                //  if (status == "OK")
                {
                    Topup topup = new Topup(null, meterId, amount, cashCards[0].Id);
                    int result;
                    result = topup.SaveData();

                    return Content(JsonConvert.SerializeObject(new { result = "sucsess", data = topup }));
                }
            }

            else if (customer.Id != meter[0].UserId && customer.CardId == cashCards[0].Id)//for another from his card
            {
                SMS sms = new SMS();
                sms.To_number = customer1.Telephone;
                sms.Msg = $"يحاول {customer.Name} شحن عدادك بقيمة {amount}";
               // string status = sms.Send();
                sms.SaveData();

                SMS sms1 = new SMS();
                sms1.To_number = customer.Telephone;
                sms1.Msg = $"يحاول {customer1.Name} شحن عداده باستخدام بطاقتك بقيمة {amount}";
                // string status1 = sms1.Send();
                sms1.SaveData();
                //if (status == "OK" && status1 == "OK")
                {

                    Topup topup = new Topup(null, meterId, amount, cashCards[0].Id);
                    int result;
                    result = topup.SaveData();
                    return Content(JsonConvert.SerializeObject(new { result = "success", data = topup }));
                }

            }


            else if (customer.Id != meter[0].UserId && customer.CardId != cashCards[0].Id && customer1.CardId != cashCards[0].Id)//for another from another card
            {
                int rrc;
                Customer[] customer2 = Customer.GetCustomers(new CustomerParameters { CardId = cashCards[0].Id }, out rrc);

                SMS sms = new SMS();
                sms.To_number = customer2[0].Telephone;
                sms.Msg = $"يحاول {customer1.Name} شحن عداده بقيمة {amount} باستخدام بطاقتك";
               // string status = sms.Send();
                sms.SaveData();

                SMS sms1 = new SMS();
                sms1.To_number = customer1.Telephone;
                sms1.Msg = $"يحاول {customer.Name} شحن عدادك بقيمة {amount} باستخدام بطاقة {customer2[0].Name}";
                // string status1 = sms1.Send();
                sms1.SaveData();
                //if (status == "OK" && status1 == "OK")
                {
                    Topup topup = new Topup(null, meterId, amount, cashCards[0].Id);
                    int result;
                    result = topup.SaveData();
                    return Content(JsonConvert.SerializeObject(new { result = "success", data = topup }));
                }
            }
            else if (customer.Id != meter[0].UserId && customer.CardId != cashCards[0].Id && customer1.CardId == cashCards[0].Id)//for another from the another card
            {

                SMS sms = new SMS();
                sms.To_number = customer1.Telephone;
                sms.Msg = $"يحاول {customer.Name} شحن عدادك باستخدام بطاقتك";
                //string status = sms.Send();
                sms.SaveData();

                SMS sms1 = new SMS();
                sms1.To_number = customer.Telephone;
                sms1.Msg = $"أهلا وسهلا بك أنت تحاول الان شحن عداد {customer1.Name} باستخدام بطاقته {customer1.CardId}";
                // string status1 = sms1.Send();
                sms1.SaveData();


                //if (status == "OK" && status1 == "OK")
                {
                    Topup topup = new Topup(null, meterId, amount, cashCards[0].Id);
                    int result;
                    result = topup.SaveData();
                    return Content(JsonConvert.SerializeObject(new { result = "success", data = topup }));
                }

            }

            return Content(JsonConvert.SerializeObject(new { result = "error" }));
        }



        public ActionResult ChargeMeterFromApp(string otp, int customerid)//this should reach the website button which charge the meter 
        {

            int rc;
            Customer customer = new Customer(customerid);
            Topup[] topup = Topup.GetTopups(new TopupParameters { OTP = otp }, out rc);
            if (topup[0].Status == "0")
            {

                Meter[] meters = Meter.GetMeters(new MeterParameters { Meterid = topup[0].MeterId }, out rc);
                customer = (Session["customer"] as Customer);
                if (customer.Id == meters[0].UserId)
                {
                    SMS sms = new SMS();
                    sms.To_number = customer.Telephone;
                    sms.Msg = $"  أهلا وسهلا بك أنت تحاول الان شحن عدادك باستخدام موقعنا في الشركة ورقم الكود الذي يريد شحنه هو {otp}";
                    string status = sms.Send();
                    sms.SaveData();

                    if (status == "OK")
                    {
                        topup[0].Charged();

                        return Content(JsonConvert.SerializeObject(new { result = "success" }));
                    }
                }

                else if (customer.Id != meters[0].UserId)
                {
                    Customer customer1 = new Customer(meters[0].UserId);
                    SMS sms = new SMS();
                    sms.To_number = customer1.Telephone;
                    sms.Msg = $"يحاول {customer.Name} شحن عدادك باستخدام موقنا في الشركة ورقم الكود الذي يريد شحنه هو {otp}";
                    //string status = sms.Send();
                    sms.SaveData();
                   // if (status == "OK")
                    {
                        topup[0].Charged();

                        return Content(JsonConvert.SerializeObject(new { result = "success" }));
                    }
                }

            }
            return Content(JsonConvert.SerializeObject(new { result = "error" }));


        }

        public ActionResult ChargeFromMainPage(string otp,int meterid, int customerid)//chargr from mainpage
        {

            int rc;
            Customer customer = new Customer(customerid);

            Meter[] meter = Meter.GetMeters(new MeterParameters { Meterid = meterid }, out rc);
            Topup[] topup = Topup.GetTopups(new TopupParameters { OTP = otp }, out rc);

            if (topup[0].Status == "0")
            {
                Meter[] meters = Meter.GetMeters(new MeterParameters { Meterid = topup[0].MeterId }, out rc);
                if (customer.Id == meters[0].UserId && customer.Id == meter[0].UserId)
                {
                    SMS sms = new SMS();
                    sms.To_number = customer.Telephone;
                    sms.Msg = $"  أهلا وسهلا بك أنت تحاول الان شحن عدادك باستخدام موقعنا في الشركة ورقم الكود الذي يريد شحنه هو {otp}";
                    string status = sms.Send();
                    sms.SaveData();

                    if (status == "OK")
                    {
                        topup[0].Charged();
                        return Content(JsonConvert.SerializeObject(new { result = "success" }));
                    }

                }

                else if (customer.Id != meters[0].UserId && customer.Id == meter[0].UserId)
                {
                        return Content(JsonConvert.SerializeObject(new { result = "error" }));
                  

                }
                else if (customer.Id != meters[0].UserId && customer.Id != meter[0].UserId)
                {
                    if (meters[0].UserId == meter[0].UserId)
                    {
                        Customer customer1 = new Customer(meters[0].UserId);
                        SMS sms = new SMS();
                        sms.To_number = customer1.Telephone;
                        sms.Msg = $"يحاول {customer.Name} شحن عدادك باستخدام موقنا في الشركة ورقم الكود الذي يريد شحنه هو {otp}";
                        string status = sms.Send();
                        sms.SaveData();
                        if (status == "OK")
                        {
                            topup[0].Charged();
                            return Content(JsonConvert.SerializeObject(new { result="succsess" }));
                        }

                    }
                    else
                    {
                        return Content(JsonConvert.SerializeObject(new { result = "succsess" }));
                    }
                }

            }
            return Content(JsonConvert.SerializeObject(new { result = "error" }));

        }

        public ActionResult ReturnOTPFromMainPage(int meterid, int customerid)
        {
            int rc;
            Customer customer = new Customer(customerid);
            Meter[] meter = Meter.GetMeters(new MeterParameters { Meterid = meterid }, out rc);

            if (customer.Id == meter[0].UserId)
            {
                SMS sms = new SMS();
                sms.To_number = customer.Telephone;
                sms.Msg = $"أهلا وسهلا بكك أنت تحاول الان استرجاع الكود الغير مشحون الخاص بك";
                string status = sms.Send();
                sms.SaveData();
                if (status == "OK")
                {
                    Topup[] topups = Topup.GetTopups(new TopupParameters { MeterId = meterid, Status = "0" }, out rc);
                    return Content(JsonConvert.SerializeObject(new { result = "sucsess", data = topups }));
                }

            }

            else if (customer.Id == meter[0].UserId)
            {

                Customer customer1 = new Customer(meter[0].UserId);

                SMS sms = new SMS();
                sms.To_number = customer1.Telephone;
                sms.Msg = $"يحاول {customer.Name} استرجاع الكود الغير مشحون الخاص بك";
                string status = sms.Send();
                sms.SaveData();
                if (status == "OK")
                {
                    Topup[] topups = Topup.GetTopups(new TopupParameters { MeterId = meterid, Status = "0" }, out rc);
                    return Content(JsonConvert.SerializeObject(new { result = "sucsess", data = topups }));
                }
            }

            return Content(JsonConvert.SerializeObject(new { result = "error" }));

        }



        public ActionResult TransferOTP(string senderOTP, int meterId,int amount, int customerid)
        {

            int rc;
            Customer customer = new Customer(customerid);
            Topup[] topup = Topup.GetTopups(new TopupParameters { OTP = senderOTP }, out rc);//get senderotp meterid 
            Meter[] meters = Meter.GetMeters(new MeterParameters { Meterid = topup[0].MeterId }, out rc);//get meterid customer

            Meter[] meters1 = Meter.GetMeters(new MeterParameters { Meterid = meterId }, out rc);// get userid will take the amount
            Customer customer1 = new Customer(meters1[0].UserId);//get customer info


            if (customer.Id == meters[0].UserId)
            {

                SMS sms = new SMS();
                sms.To_number = customer.Telephone;
                sms.Msg = $"أهلا وسلا بك في تطبيقنا أنت تحاول الان تحويل قيمة {amount} الى حساب {customer1.Name} ";
                string status = sms.Send();
                sms.SaveData();

                SMS sms1 = new SMS();
                sms.To_number = customer1.Telephone;
                sms.Msg = $"يحاول {customer.Name} تحويل قيمة {amount} الى عدادك";
                string status1 = sms1.Send();
                sms1.SaveData();
                if (status == "OK" && status1 == "OK")
                {
                    Transfer transfer = new Transfer(null, senderOTP, meterId, amount);
                    int result;
                    result = transfer.SaveData();
                    return Content(JsonConvert.SerializeObject(new { result = "sucess", data = transfer }));

                }
            }

            else if (customer.Id != meters[0].UserId)
            {
                Customer customer2 = new Customer(meters[0].UserId);
                SMS sms = new SMS();
                sms.To_number = customer2.Telephone;
                sms.Msg = $"أهلا وسلا بك في تطبيقنا أنت تحاول الان تحويل قيمة {amount} الي حساب {customer1.Name} ";
                string status = sms.Send();
                sms.SaveData();

                SMS sms1 = new SMS();
                sms.To_number = customer1.Telephone;
                sms.Msg = $"يحاول {customer2.Name} تحويل قيمة {amount} الى عدادك";
                string status1 = sms1.Send();
                sms1.SaveData();
                if (status == "OK" && status1 == "OK")
                {
                    Transfer transfer = new Transfer(null, senderOTP, meterId, amount);
                    int result;
                    result = transfer.SaveData();
                    return Content(JsonConvert.SerializeObject(new { result = "sucess", data = transfer }));
                }
            }
            return Content(JsonConvert.SerializeObject(new { result = "error" }));
        }

        public ActionResult ChargeHist(DateTime fromdate, DateTime todate, int customerid)
        {
            int rc;
            Customer customer = new Customer(customerid);
            Meter[] meters = Meter.GetMeters(new MeterParameters { UserId = customer.Id }, out rc);

            Topup[] topups = Topup.GetTopups(new TopupParameters { fromdate = fromdate, todate = todate, MeterId = meters[0].Meterid }, out rc);

            decimal? amount = 0;
            decimal? count = 0;
            foreach (Topup topup in topups)
            {
                amount += topup.Amount;
                count += 1;
            }
            return Content(JsonConvert.SerializeObject(new { result = "success", data = amount, count }));
        }


        public ActionResult ChargingHist(int customerid)
        {
            int rc;
            Customer customer = new Customer(customerid);
            Meter[] meters = Meter.GetMeters(new MeterParameters { UserId = customer.Id }, out rc);
            int? meterid = meters[0].Meterid;
           Topup[] topups = Topup.GetTopups(new TopupParameters { MeterId = meterid }, out rc);
            return Content(JsonConvert.SerializeObject(new { result = "success", data = topups }));
        }


        public ActionResult TransferHist(int customerid)
        {
            int rc;
            Customer customer = new Customer(customerid);
            Meter[] meters = Meter.GetMeters(new MeterParameters { UserId = customer.Id }, out rc);

            Transfer[] transfers = Transfer.GetTransfers(new TransferParameters { MeterId = meters[0].Meterid }, out rc);
            Transfer[] transfers2 = Transfer.GetTransfersBySenderOTP(new TransferParameters { MeterId = meters[0].Meterid }, out rc);

            if (transfers != null && transfers2 == null)
            {
                return Content(JsonConvert.SerializeObject(new { result = "success", data = transfers }));
            }
            if (transfers == null && transfers2 != null)
            {
                return Content(JsonConvert.SerializeObject(new { result = "success", data = transfers2 }));
            }
            if (transfers != null && transfers2 != null)
            {
                return Content(JsonConvert.SerializeObject(new { result = "success", data = transfers2, transfers }));
            }
            return Content(JsonConvert.SerializeObject(new { result = "error"}));
        }
    }
}
