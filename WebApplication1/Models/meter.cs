using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class meter
    {
        public int? id { get; set; }
        public int? user_id { get; set; }
        public decimal? amount { get; set; }

        public meter(int ID)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "GetMetersByID";
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    r.Read();
                    if (r["id"] != DBNull.Value)
                    {
                        this.id = Convert.ToInt32(r["id"]);
                        this.user_id = Convert.ToInt32(r["user_id"]);
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
                cmd.CommandText = "SaveMeterData";

                if (user_id != null)
                    cmd.Parameters.AddWithValue("user_id", user_id);

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