using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class cashCard
    {
        public int? id { get; set; }
        public string password { get; set; }
        public int? customer_id { get; set; }
        public decimal amount { get; set; }


        public cashCard(int ID)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "GetcashCard";
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    r.Read();
                    if (r["id"] != DBNull.Value)
                    {
                        this.id = Convert.ToInt32(r["id"]);
                        this.password = Convert.ToString(r["password"]);
                        this.customer_id = Convert.ToInt32(r["customer_id"]);
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
                cmd.CommandText = "SavecashCardData";


                if (password != null)
                    cmd.Parameters.AddWithValue("password", password);

                if (customer_id != null)
                    cmd.Parameters.AddWithValue("customer_id", customer_id);


                if (amount!= null)
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