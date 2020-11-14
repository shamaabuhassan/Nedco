using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{

    public class CashCardParameters
    {
        public int? CustomerId { get; set; }
        public int? Id { get; set; }
        public string Password { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }

        public int? Cardid { get; set; }


    }
    public class CashCard
    {
        public int? Id { get; set; }
        public string Password { get; set; }
        public int? CustomerId { get; set; }
        public decimal ?Amount { get; set; }
        public string Status { get; set; }
        public int? Cardid { get; set; }
        //get element bu=y id 
        public CashCard(int id)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "GetCashCardById";
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    r.Read();
                    if (r["id"] != DBNull.Value) this.Id = Convert.ToInt32(r["id"]);
                    this.Password = Convert.ToString(r["password"]);
                    if (r["customer_id"] != DBNull.Value) this.CustomerId = Convert.ToInt32(r["customer_id"]);
                    if (r["amount"] != DBNull.Value) this.Amount = Convert.ToDecimal(r["amount"]);
                    if (r["card_id"] != DBNull.Value) this.Id = Convert.ToInt32(r["card_id"]);
                    cmd.Connection.Close();


                }
            }
        }

        //constructor
        public CashCard(int?id, string password, int? customerId, decimal? amount,int? cardid)
        {
            this.Id = id;
            this.Password = password;
            this.CustomerId = customerId;
            this.Amount = amount;
            this.Cardid = cardid;

        }

        public void Delete()
        {
            this.Status = "3";
            this.SaveData();
        }
        public int SaveData()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "SaveCashCardData";


               
                if (Password != null) cmd.Parameters.AddWithValue("password", Password);
                if (CustomerId != null) cmd.Parameters.AddWithValue("customer_id", CustomerId);
                if (Amount != null) cmd.Parameters.AddWithValue("amount", Amount);
                if (Cardid != null) cmd.Parameters.AddWithValue("card_id", Cardid);


                SqlParameter idParam = cmd.Parameters.Add("@id", SqlDbType.Int);
                idParam.Direction = ParameterDirection.InputOutput;

                SqlParameter resultParm = cmd.Parameters.Add("@result", SqlDbType.Int);
                resultParm.Direction = ParameterDirection.InputOutput;

                idParam.Value = this.Id;

                int c = cmd.ExecuteNonQuery();


                this.Id = Convert.ToInt32(idParam.Value);
                int result = Convert.ToInt32(resultParm.Value);
                cmd.Connection.Close();
                return result;

            }
        }
        public CashCard()
        { }

        //get data using list
        public static CashCard[] GetCashCards(CashCardParameters parameters, out int rowsCount)
        {
            List<CashCard> l = new List<CashCard>();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "GetCashCard";

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        //create object of the class
                        CashCard c = new CashCard();
                        if (r["id"] != DBNull.Value) c.Id = Convert.ToInt32(r["id"]);
                        if (r["password"] != DBNull.Value) c.Password = Convert.ToString(r["password"]);
                        if (r["customer_id"] != DBNull.Value) c.CustomerId = Convert.ToInt32(r["customer_id"]);
                        if (r["amount"] != DBNull.Value) c.Amount = Convert.ToDecimal(r["amount"]);

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