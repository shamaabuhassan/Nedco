using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebApplication1.Models
{
    public class TopupParameters
    {
        public int? Id { get; set; }
        public int? MeterId { get; set; }
        public decimal? Amount { get; set; }
        public int? CardId { get; set; }
        public string OTP { get; set; }
        public DateTime? ChargeDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public string Status { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

    }
    

        public class Topup
        {
            public int? Id { get; set; }
            public int? MeterId { get; set; }
            public decimal? Amount { get; set; }
            public int? CardId { get; set; }
            public string OTP { get; set; }
            public DateTime? ChargeDate { get; set; }
            public DateTime? ActivationDate { get; set; }
            public string Status { get; set; }

            public Topup() { }

          
        public void Charged()
        {
            ActivationDate = DateTime.Now;
            using (SqlCommand cmd=new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "Charged";


                if (this.MeterId != null) cmd.Parameters.AddWithValue("meter_id", this.MeterId);
                if (this.Amount != null) cmd.Parameters.AddWithValue("amount", this.Amount);
                if (this.CardId != null) cmd.Parameters.AddWithValue("card_id", this.CardId);
                if (this.OTP != null) cmd.Parameters.AddWithValue("otp", this.OTP);
                if (this.ChargeDate != null) cmd.Parameters.AddWithValue("chargeDate", this.ChargeDate);
                if (this.ActivationDate != null) cmd.Parameters.AddWithValue("activationDate", ActivationDate);
                if (this.Status != null) cmd.Parameters.AddWithValue("status", this.Status);

                int c = cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }
        }
            public Topup(int? id)
            {

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = new SqlConnection(cstr.con);
                    cmd.Connection.Open();
                    cmd.CommandText = "GetTopupsByID";
                    cmd.Parameters.AddWithValue("@ID", id);

                    SqlDataReader r = cmd.ExecuteReader();
                    if (r.HasRows)
                    {
                        r.Read();
                        if (r["id"] != DBNull.Value) this.Id = Convert.ToInt32(r["id"]);
                        if (r["meter_id"] != DBNull.Value) this.MeterId = Convert.ToInt32(r["meter_id"]);
                        if (r["amount"] != DBNull.Value) this.Amount = Convert.ToDecimal(r["amount"]);
                        if (r["card_id"] != DBNull.Value) this.CardId = Convert.ToInt32(r["card_id"]);
                        this.OTP = Convert.ToString(r["otp"]);
                        if (r["chargeDate"] != DBNull.Value) this.ChargeDate = Convert.ToDateTime(r["chargeDate"]);
                        if (r["activationDate"] != DBNull.Value) this.ActivationDate = Convert.ToDateTime(r["activationDate"]);
                        this.Status = Convert.ToString(r["status"]);
                        cmd.Connection.Close();
                    }
                }
            }

        public Topup(string otp)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "GetTopupsByOTP";
                cmd.Parameters.AddWithValue("@otp", otp);

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    r.Read();
                    if (r["id"] != DBNull.Value) this.Id = Convert.ToInt32(r["id"]);
                    if (r["meter_id"] != DBNull.Value) this.MeterId = Convert.ToInt32(r["meter_id"]);
                    if (r["amount"] != DBNull.Value) this.Amount = Convert.ToDecimal(r["amount"]);
                    if (r["card_id"] != DBNull.Value) this.CardId = Convert.ToInt32(r["card_id"]);
                    this.OTP = Convert.ToString(r["otp"]);
                    if (r["chargeDate"] != DBNull.Value) this.ChargeDate = Convert.ToDateTime(r["chargeDate"]);
                    if (r["activationDate"] != DBNull.Value) this.ActivationDate = Convert.ToDateTime(r["activationDate"]);
                    this.Status = Convert.ToString(r["status"]);
                    cmd.Connection.Close();
                }
            }
        }

        //constructor

        public Topup(int? id, int? meterId, decimal? amount, int? cardId)
            {
                this.Id = id;
                this.MeterId = meterId;
                this.Amount = amount;
                this.CardId = cardId;

            }
        public Topup( int? meterId, decimal? amount, int? cardId)
        {
       
            this.MeterId = meterId;
            this.Amount = amount;
            this.CardId = cardId;

        }

        public void Delete()
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "DeleteTopup";
                cmd.Parameters.AddWithValue("id", this.Id);

                 cmd.ExecuteNonQuery();


            }

        }

        public string GenerateRandom()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            return r;
        }
        public int SaveData()

            {
            CashCard cashCard = new CashCard(CardId);
            int result = 0;
            Customer customer = new Customer(CardId);
              if (cashCard.Amount > Amount)
                {
                    OTP = GenerateRandom();
                    ChargeDate = DateTime.Now;
                    Status = "0";

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = new SqlConnection(cstr.con);
                        cmd.Connection.Open();
                        cmd.CommandText = "SaveTopupData";


                        if (MeterId != null) cmd.Parameters.AddWithValue("meter_id", MeterId);
                        if (Amount != null) cmd.Parameters.AddWithValue("amount", Amount);
                        if (CardId != null) cmd.Parameters.AddWithValue("card_id", CardId);
                        if (OTP != null) cmd.Parameters.AddWithValue("otp", OTP);
                        if (ChargeDate != null) cmd.Parameters.AddWithValue("chargeDate", ChargeDate);
                        if (ActivationDate != null) cmd.Parameters.AddWithValue("activationDate", ActivationDate);
                        if (Status != null) cmd.Parameters.AddWithValue("status", Status);

                        SqlParameter idParam = cmd.Parameters.Add("@id", SqlDbType.Int);
                        idParam.Direction = ParameterDirection.InputOutput;

                        SqlParameter resultParam = cmd.Parameters.Add("@result", SqlDbType.Int);
                        resultParam.Direction = ParameterDirection.InputOutput;

                        idParam.Value = this.Id;

                        int c = cmd.ExecuteNonQuery();

                        this.Id = Convert.ToInt32(idParam.Value);
                        result = Convert.ToInt32(resultParam.Value);
                        cmd.Connection.Close();

                    }
                    decimal? amount = cashCard.Amount - Amount;
                    CashCard cash = new CashCard(cashCard.Id, cashCard.Password, amount, cashCard.Cardid);
                    cash.SaveData();
                
            }
            return result;
           
        }

        
        public static Topup[] GetTopups(TopupParameters parameters, out int rowsCount)
            {
                List<Topup> l = new List<Topup>();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = new SqlConnection(cstr.con);
                    cmd.Connection.Open();
                    cmd.CommandText = "GetTopups";
                cmd.Parameters.AddWithValue("@card_id", parameters.CardId);
                cmd.Parameters.AddWithValue("@status", parameters.Status);
                cmd.Parameters.AddWithValue("@otp", parameters.OTP);
                cmd.Parameters.AddWithValue("@month", parameters.Month);
                cmd.Parameters.AddWithValue("@year", parameters.Year);
                cmd.Parameters.AddWithValue("@meter_id", parameters.MeterId);

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                    {
                        while (r.Read())
                        {
                            Topup c = new Topup();
                            if (r["id"] != DBNull.Value) c.Id = Convert.ToInt32(r["id"]);
                            if (r["meter_id"] != DBNull.Value) c.MeterId = Convert.ToInt32(r["meter_id"]);
                            if (r["amount"] != DBNull.Value) c.Amount = Convert.ToDecimal(r["amount"]);
                            if (r["card_id"] != DBNull.Value) c.CardId = Convert.ToInt32(r["card_id"]);
                            if (r["otp"] != DBNull.Value) c.OTP = Convert.ToString(r["otp"]);
                            if (r["chargeDate"] != DBNull.Value) c.ChargeDate = Convert.ToDateTime(r["chargeDate"]);
                            if (r["activationDate"] != DBNull.Value) c.ActivationDate = Convert.ToDateTime(r["activationDate"]);
                            if (r["status"] != DBNull.Value) c.Status = Convert.ToString(r["status"]);

                            l.Add(c);
                        }
                    }

                    r.Close();
                    cmd.Connection.Close();
                    rowsCount = l.Count;
                }
                return l.ToArray();

            }

        }
    }
