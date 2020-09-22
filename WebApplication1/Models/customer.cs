using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class customer
    {
        public int? id { get; set; }
        public string username { get; set; }
        public int? meter_id { get; set; }
        public int? card_id { get; set; }
        public string telephone { get; set; }
        public int? country_id { get; set; }
        public int? city_id { get; set; }
        public string area { get; set; }
        public string street { get; set; }
        public string password { get; set; }


        public customer(int ID)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "GetCustomerByID";
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    r.Read();
                    if (r["id"] != DBNull.Value)
                    {
                        this.id = Convert.ToInt32(r["id"]);
                        this.username = Convert.ToString(r["username"]);
                        this.meter_id = Convert.ToInt32(r["meter_id"]);
                        this.card_id = Convert.ToInt32(r["card_id"]);
                        this.telephone = Convert.ToString(r["telephone"]);
                        this.country_id = Convert.ToInt32(r["country_id"]);
                        this.city_id = Convert.ToInt32(r["city_id"]);
                        this.area = Convert.ToString(r["area"]);
                        this.street = Convert.ToString(r["street"]);
                        this.password = Convert.ToString(r["password"]);

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
                cmd.CommandText = "SaveCustomerData";

                if (username != null)
                    cmd.Parameters.AddWithValue("username", username);

                if (meter_id != null)
                    cmd.Parameters.AddWithValue("meter_id", meter_id);


                if (card_id.ToString() != null)
                    cmd.Parameters.AddWithValue("card_id", card_id);


                if (telephone != null)
                    cmd.Parameters.AddWithValue("telephone", telephone);

                if (country_id.ToString() != null)
                    cmd.Parameters.AddWithValue("country_id", country_id);

                if (city_id != null)
                    cmd.Parameters.AddWithValue("city_id", city_id);


                if (area != null)
                    cmd.Parameters.AddWithValue("area", area);


                if (street != null)
                    cmd.Parameters.AddWithValue("street", street);


                if (password != null)
                    cmd.Parameters.AddWithValue("password", password);

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