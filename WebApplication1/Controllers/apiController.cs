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
        public ActionResult CheckLogin(string Username, string Password)
        {
            Customer customer = Customer.CheckLogin(Username, Password);
            if (customer != null)
            {
                return Content(JsonConvert.SerializeObject(new { result = "success", data = customer }));
            }
            else
            {
                return Content(JsonConvert.SerializeObject(new { result = "error", data = "the username or password is wrong" }));
            }

        }

        public ActionResult RequestOTP(int? MeterId, int? Amount, int SerialNumber, int customerid)
        //this should reach the website button which request OTP 

        {
            int rc;
            Customer customer = new Customer(customerid);
            Meter[] meter = Meter.GetMeters(new MeterParameters { Meterid = MeterId }, out rc);//user of meter
            CashCard[] cashCards = CashCard.GetCashCards(new CashCardParameters { SerialNumber = SerialNumber }, out rc);
            Customer customer1=null;
            if (meter != null)
            {
                 customer1 = new Customer(meter[0].UserId.Value);
            }

            if (customer.Id == meter[0].UserId && customer.CardId == cashCards[0].Id)//for himself from his card
            {
                SMS sms = new SMS();
                sms.To_number = customer.Telephone;
                sms.Msg = $"أهلا وسهلا بك أنت تقوم بشحن {Amount} الى عدادك الان";
               // string status = sms.Send();
                
               // if(status == "OK")
                {
                    Topup topup = new Topup(null, MeterId, Amount, cashCards[0].SerialNumber);
                    
                    int result;
                    result = topup.SaveData();
                    if (result == 1)
                    {
                        return Content(JsonConvert.SerializeObject(new { result = "success", data = topup }));
                    }
                }
                
            }

            else if (customer.Id == meter[0].UserId && customer.CardId != cashCards[0].Id) //for himself from another card
            {

                Customer[] customer2 = Customer.GetCustomers(new CustomerParameters { CardId = cashCards[0].Id }, out rc);
                SMS sms = new SMS();
                sms.To_number = customer2[0].Telephone;
                sms.Msg = $" يحاول {customer.Name} شحن عداده باستخدام البطاقة الخاصة بك بقيمة {Amount}";
                //  string status = sms.Send();
                
                //  if (status == "OK")
                {
                    Topup topup = new Topup(null, MeterId, Amount, cashCards[0].Id);
                    int result;
                    result = topup.SaveData();
                    if (result == 1)
                    {
                        return Content(JsonConvert.SerializeObject(new { result = "sucsess", data = topup }));
                    }
                }
            }

            else if (customer.Id != meter[0].UserId && customer.CardId == cashCards[0].Id)//for another from his card
            {
                SMS sms = new SMS();
                sms.To_number = customer1.Telephone;
                sms.Msg = $"يحاول {customer.Name} شحن عدادك بقيمة {Amount}";
               // string status = sms.Send();
                

                SMS sms1 = new SMS();
                sms1.To_number = customer.Telephone;
                sms1.Msg = $"يحاول {customer1.Name} شحن عداده باستخدام بطاقتك بقيمة {Amount}";
                // string status1 = sms1.Send();
                
                //if (status == "OK" && status1 == "OK")
                {

                    Topup topup = new Topup(null, MeterId, Amount, cashCards[0].Id);
                    int result;
                    result = topup.SaveData();
                    if (result == 1)
                    {
                        return Content(JsonConvert.SerializeObject(new { result = "success", data = topup }));
                    }
                }

            }


            else if (customer.Id != meter[0].UserId && customer.CardId != cashCards[0].Id && customer1.CardId != cashCards[0].Id)//for another from another card
            {
                int rrc;
                Customer[] customer2 = Customer.GetCustomers(new CustomerParameters { CardId = cashCards[0].Id }, out rrc);

                SMS sms = new SMS();
                sms.To_number = customer2[0].Telephone;
                sms.Msg = $"يحاول {customer1.Name} شحن عداده بقيمة {Amount} باستخدام بطاقتك";
               // string status = sms.Send();
                

                SMS sms1 = new SMS();
                sms1.To_number = customer1.Telephone;
                sms1.Msg = $"يحاول {customer.Name} شحن عدادك بقيمة {Amount} باستخدام بطاقة {customer2[0].Name}";
                // string status1 = sms1.Send();
                
                //if (status == "OK" && status1 == "OK")
                {
                    Topup topup = new Topup(null, MeterId, Amount, cashCards[0].Id);
                    int result;
                    result = topup.SaveData();
                    if (result == 1)
                    {
                        return Content(JsonConvert.SerializeObject(new { result = "success", data = topup }));
                    }
                }
            }
            else if (customer.Id != meter[0].UserId && customer.CardId != cashCards[0].Id && customer1.CardId == cashCards[0].Id)//for another from the another card
            {

                SMS sms = new SMS();
                sms.To_number = customer1.Telephone;
                sms.Msg = $"يحاول {customer.Name} شحن عدادك باستخدام بطاقتك";
                //string status = sms.Send();
             

                SMS sms1 = new SMS();
                sms1.To_number = customer.Telephone;
                sms1.Msg = $"أهلا وسهلا بك أنت تحاول الان شحن عداد {customer1.Name} باستخدام بطاقته {customer1.CardId}";
                // string status1 = sms1.Send();
               
                

                //if (status == "OK" && status1 == "OK")
                {
                    Topup topup = new Topup(null, MeterId, Amount, cashCards[0].Id);
                    int result;
                    result = topup.SaveData();
                    if (result == 1)
                    {
                        return Content(JsonConvert.SerializeObject(new { result = "success", data = topup }));
                    }
                }

            }

            return Content(JsonConvert.SerializeObject(new { result = "error" }));
        }



        public ActionResult ChargeMeterFromApp(string OTP, int customerid)//this should reach the website button which charge the meter 
        {

            int rc;
            Customer customer = new Customer(customerid);
            Topup[] topup = Topup.GetTopups(new TopupParameters { OTP = OTP }, out rc);
            if (topup[0].Status == "0")
            {

                Meter[] meters = Meter.GetMeters(new MeterParameters { Meterid = topup[0].MeterId }, out rc);

                if (customer.Id == meters[0].UserId)
                {
                    SMS sms = new SMS();
                    sms.To_number = customer.Telephone;
                    sms.Msg = $"  أهلا وسهلا بك أنت تحاول الان شحن عدادك باستخدام موقعنا في الشركة ورقم الكود الذي يريد شحنه هو {OTP}";
                   // string status = sms.Send();
                    

                   // if (status == "OK")
                    {
                        topup[0].Charged();

                        return Content(JsonConvert.SerializeObject(new { result = "success" }));
                    }
                }

                else if (customer.Id != meters[0].UserId)
                {
                    Customer customer1 = new Customer(meters[0].UserId.Value);
                    SMS sms = new SMS();
                    sms.To_number = customer1.Telephone;
                    sms.Msg = $"يحاول {customer.Name} شحن عدادك باستخدام موقنا في الشركة ورقم الكود الذي يريد شحنه هو {OTP}";
                    //string status = sms.Send();
                  
                   // if (status == "OK")
                    {
                        topup[0].Charged();

                        return Content(JsonConvert.SerializeObject(new { result = "success" }));
                    }
                }

            }
            return Content(JsonConvert.SerializeObject(new { result = "error" }));


        }

        public ActionResult ChargeFromMainPage(string OTP,int Meterid, int customerid)//chargr from mainpage
        {

            int rc;
            Customer customer = new Customer(customerid);

            Meter[] meter = Meter.GetMeters(new MeterParameters { Meterid = Meterid }, out rc);
            Topup[] topup = Topup.GetTopups(new TopupParameters { OTP = OTP }, out rc);

            if (topup[0].Status == "0")
            {
                Meter[] meters = Meter.GetMeters(new MeterParameters { Meterid = topup[0].MeterId }, out rc);
                if (customer.Id == meters[0].UserId && customer.Id == meter[0].UserId)
                {
                    SMS sms = new SMS();
                    sms.To_number = customer.Telephone;
                    sms.Msg = $"  أهلا وسهلا بك أنت تحاول الان شحن عدادك باستخدام موقعنا في الشركة ورقم الكود الذي يريد شحنه هو {OTP}";
                    //string status = sms.Send();
                   

                   // if (status == "OK")
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
                        Customer customer1 = new Customer(meters[0].UserId.Value);
                        SMS sms = new SMS();
                        sms.To_number = customer1.Telephone;
                        sms.Msg = $"يحاول {customer.Name} شحن عدادك باستخدام موقنا في الشركة ورقم الكود الذي يريد شحنه هو {OTP}";
                        //string status = sms.Send();
                     
                       // if (status == "OK")
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

        public ActionResult ReturnOTPFromMainPage(int Meterid, int customerid)
        {
            int rc;
            Customer customer = new Customer(customerid);
            Meter[] meter = Meter.GetMeters(new MeterParameters { Meterid = Meterid }, out rc);

            if (customer.Id == meter[0].UserId)
            {
                SMS sms = new SMS();
                sms.To_number = customer.Telephone;
                sms.Msg = $"أهلا وسهلا بكك أنت تحاول الان استرجاع الكود الغير مشحون الخاص بك";
                //string status = sms.Send();
                
               // if (status == "OK")
                {
                    Topup[] topups = Topup.GetTopups(new TopupParameters { MeterId = Meterid, Status = "0" }, out rc);
                    return Content(JsonConvert.SerializeObject(new { result = "success", data = topups }));
                }

            }

            else if (customer.Id == meter[0].UserId)
            {

                Customer customer1 = new Customer(meter[0].UserId.Value);

                SMS sms = new SMS();
                sms.To_number = customer1.Telephone;
                sms.Msg = $"يحاول {customer.Name} استرجاع الكود الغير مشحون الخاص بك";
               // string status = sms.Send();
               
               // if (status == "OK")
                {
                    Topup[] topups = Topup.GetTopups(new TopupParameters { MeterId = Meterid, Status = "0" }, out rc);
                    return Content(JsonConvert.SerializeObject(new { result = "success", data = topups }));
                }
            }

            return Content(JsonConvert.SerializeObject(new { result = "error" }));

        }



        public ActionResult TransferOTP(string SenderOTP, int MeterId,int Amount, int customerid)
        {

            int rc;
            Customer customer = new Customer(customerid);
            Topup[] topup = Topup.GetTopups(new TopupParameters { OTP = SenderOTP }, out rc);//get senderotp meterid 
            Meter[] meters = Meter.GetMeters(new MeterParameters { Meterid = topup[0].MeterId }, out rc);//get meterid customer

            Meter[] meters1 = Meter.GetMeters(new MeterParameters { Meterid = MeterId }, out rc);// get userid will take the amount
            Customer customer1 = new Customer(meters1[0].UserId.Value);//get customer info


            if (customer.Id == meters[0].UserId)
            {

                SMS sms = new SMS();
                sms.To_number = customer.Telephone;
                sms.Msg = $"أهلا وسلا بك في تطبيقنا أنت تحاول الان تحويل قيمة {Amount} الى حساب {customer1.Name} ";
              //  string status = sms.Send();
              

                SMS sms1 = new SMS();
                sms.To_number = customer1.Telephone;
                sms.Msg = $"يحاول {customer.Name} تحويل قيمة {Amount} الى عدادك";
               // string status1 = sms1.Send();
               
                //if (status == "OK" && status1 == "OK")
                {
                    Transfer transfer = new Transfer(null, SenderOTP, MeterId, Amount);

                    Topup[] topupp = new Topup[] { };
                    
                    topupp=transfer.SaveData();
                     
                        return Content(JsonConvert.SerializeObject(new { result = "success", data = topupp }));
                    

                }
            }

            else if (customer.Id != meters[0].UserId)
            {
                Customer customer2 = new Customer(meters[0].UserId.Value);
                SMS sms = new SMS();
                sms.To_number = customer2.Telephone;
                sms.Msg = $"أهلا وسلا بك في تطبيقنا أنت تحاول الان تحويل قيمة {Amount} الي حساب {customer1.Name} ";
                //string status = sms.Send();
                

                SMS sms1 = new SMS();
                sms.To_number = customer1.Telephone;
                sms.Msg = $"يحاول {customer2.Name} تحويل قيمة {Amount} الى عدادك";
               // string status1 = sms1.Send();
              
               // if (status == "OK" && status1 == "OK")
                {
                    Transfer transfer = new Transfer(null, SenderOTP, MeterId, Amount);

                    Topup[] topupp = new Topup[] { };

                    topupp = transfer.SaveData();
                    return Content(JsonConvert.SerializeObject(new { result = "success", data = topupp }));
                }
            }
            return Content(JsonConvert.SerializeObject(new { result = "error" }));
        }


        public ActionResult ChargeHist(DateTime fromdate, DateTime todate, int customerid)
        {
            int rc;
            Customer customer = new Customer(customerid);
            Meter[] meters = Meter.GetMeters(new MeterParameters { UserId = customer.Id }, out rc);

            List<Topup> topups1 = new List<Topup>();

            foreach (Meter meter in meters)
            {
                Topup[] topups = Topup.GetTopups(new TopupParameters { fromdate = fromdate, todate = todate, MeterId = meter.Meterid }, out rc);
                foreach (Topup topup in topups)
                {
                    topups1.Add(topup);
                }
            }

            decimal? amount = 0;
            decimal? count = 0;
            foreach (Topup topup in topups1)
            {
                amount += topup.Amount;
                count += 1;
            }
            decimal? result = amount / count;
            return Content(JsonConvert.SerializeObject(new { result = "success", data = result }));
        }


        public ActionResult ChargingHist(int customerid)
        {
            int rc;
            Customer customer = new Customer(customerid);
            Meter[] meters = Meter.GetMeters(new MeterParameters { UserId = customer.Id }, out rc);
            //  Topup[] topups = null;
            List<Topup> topups = new List<Topup>();
            //int c = 0;
            foreach (Meter meter in meters) {
                int? meterid = meter.Meterid;
                Topup[] topup = Topup.GetTopups(new TopupParameters { MeterId = meterid }, out rc);
                foreach (Topup topup1 in topup)
                {
                    topups.Add(topup1);
                }
              //  c++;
            }
            return Content(JsonConvert.SerializeObject(new { result = "success", data = topups }));
        }


        public ActionResult TransferHist(int customerid)
        {
            int rc;
            Customer customer = new Customer(customerid);
            Meter[] meters = Meter.GetMeters(new MeterParameters { UserId = customer.Id }, out rc);

            List<Transfer> transfers1 = new List<Transfer>();
            foreach (Meter meter in meters)
            {
                int? meterid = meter.Meterid;
                Transfer[] transfer = Transfer.GetTransfers(new TransferParameters { MeterId = meters[0].Meterid }, out rc);
                foreach (Transfer transfer1 in transfer)
                {
                    transfers1.Add(transfer1);
                }
                
            }

            List<Transfer> transfers2 = new List<Transfer>();
            foreach (Meter meter in meters)
            {
                int? meterid = meter.Meterid;
                Transfer[] transfert = Transfer.GetTransfersBySenderOTP(new TransferParameters { MeterId = meters[0].Meterid }, out rc);
                foreach (Transfer transfer2 in transfert)
                {
                    transfers2.Add(transfer2);
                }
                
            }
            if (transfers1 != null && transfers2 == null)
            {
                return Content(JsonConvert.SerializeObject(new { result = "success", data =transfers1 }));
            }
            if (transfers1 == null && transfers2 != null)
            {
                return Content(JsonConvert.SerializeObject(new { result = "success", data = transfers2}));
            }
            if (transfers1 != null && transfers2 != null)
            {
                return Content(JsonConvert.SerializeObject(new { result = "success", data = transfers2, transfers1 }));
            }
            return Content(JsonConvert.SerializeObject(new { result = "error"}));
        }
    }
}
