using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class transfer
    {
        public int? id { get; set; }
        public string senderOTP { get; set; }
        public int? meter_id { get; set; }
        public decimal? amount { get; set; }


        public transfer(int ID)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "GetTransfersByID";
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    r.Read();
                    if (r["id"] != DBNull.Value)
                    {
                        this.id = Convert.ToInt32(r["id"]);
                        this.senderOTP = Convert.ToString(r["senderOTP"]);
                        this.meter_id = Convert.ToInt32(r["meter_id"]);
                        this.amount = Convert.ToInt32(r["amount"]);


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
                cmd.CommandText = "SaveTransferData";

                if (senderOTP != null)
                    cmd.Parameters.AddWithValue("senderOTP", senderOTP);

                if (meter_id != null)
                    cmd.Parameters.AddWithValue("meter_id", meter_id);

                if (amount != null)
                    cmd.Parameters.AddWithValue("amount", amount);



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