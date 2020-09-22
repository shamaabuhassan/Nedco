using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class topup
    {
        public int? id { get; set; }
        public int? meter_id { get; set; }
        public decimal? amount { get; set; }
        public int? card_id { get; set; }
        public string otp { get; set; }
        public DateTime chargeDate { get; set; }
        public DateTime activationDate { get; set; }
        public string status { get; set; }

        public topup(int ID)
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
                    if (r["id"] != DBNull.Value)
                    {
                        this.id = Convert.ToInt32(r["id"]);
                        this.meter_id = Convert.ToInt32(r["meter_id"]);
                        this.amount = Convert.ToInt32(r["amount"]);
                        this.card_id = Convert.ToInt32(r["card_id"]);
                        this.otp = Convert.ToString(r["otp"]);
                        this.chargeDate = Convert.ToDateTime(r["chargeDate"]);
                        this.activationDate = Convert.ToDateTime(r["activationDate"]);
                        this.status = Convert.ToString(r["status"]);



                    }
                    cmd.Connection.Close();


                }
            }
        }


        public int SavwData()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "SaveTopupData";


                if (meter_id != null)
                    cmd.Parameters.AddWithValue("meter_id", meter_id);


                if (amount != null)
                    cmd.Parameters.AddWithValue("amount", amount);

                if (card_id != null)
                    cmd.Parameters.AddWithValue("card_id", card_id);



                if (otp != null)
                    cmd.Parameters.AddWithValue("otp", otp);

                if (chargeDate != null)
                    cmd.Parameters.AddWithValue("chargeDate", chargeDate);


                if (activationDate != null)
                    cmd.Parameters.AddWithValue("activationDate",activationDate);


             

                SqlParameter resultParm = cmd.Parameters.Add("@result", SqlDbType.Int);
                resultParm.Direction = ParameterDirection.InputOutput;

                int c = cmd.ExecuteNonQuery();
                int result = Convert.ToInt32(resultParm.Value);
                cmd.Connection.Close();
                return result;

            }
        }
    }
}