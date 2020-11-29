using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CustomerParameters
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public int? CardId { get; set; }
        public string Telephone { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string Password { get; set; }
        public string name { get; set; }

    }
    public class Customer
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public int? CardId { get; set; }
        public string Telephone { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string Password { get; set; }
        public string name { get; set; }

        public static Customer CheckLogin(string username,string password) {



            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "CheckLogin";

                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);



                SqlParameter idParam = cmd.Parameters.Add("@user_id", SqlDbType.Int);
                idParam.Direction = ParameterDirection.InputOutput;

       

                int c = cmd.ExecuteNonQuery();

                int customerid = Convert.ToInt32(idParam.Value);
                cmd.Connection.Close();
                Customer customer;
                if (customerid != 0)
                {
                    customer = new Customer(customerid);
                    
                        }
                else
                {
                    customer = null;
                }

                return customer;
            }


        }
            public Customer(int ID)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "GetCustomerByID";
                cmd.Parameters.AddWithValue("@ID", ID);

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    r.Read();
                    if (r["id"] != DBNull.Value) this.Id = Convert.ToInt32(r["id"]);
                    this.Username = Convert.ToString(r["username"]);
                    if (r["card_id"] != DBNull.Value) this.CardId = Convert.ToInt32(r["card_id"]);
                    this.Telephone = Convert.ToString(r["telephone"]);
                    if (r["country_id"] != DBNull.Value) this.CountryId = Convert.ToInt32(r["country_id"]);
                    if (r["city_id"] != DBNull.Value) this.CityId = Convert.ToInt32(r["city_id"]);
                    this.Town = Convert.ToString(r["town"]);
                    this.Street = Convert.ToString(r["street"]);
                    this.Password = Convert.ToString(r["password"]);
                    this.name = Convert.ToString(r["name"]);

                    cmd.Connection.Close();


                }
            }
        }

        public Customer(int ?CardID)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "GetCustomerByCardID";
                cmd.Parameters.AddWithValue("@CARDID", CardID);

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    r.Read();
                    if (r["id"] != DBNull.Value) this.Id = Convert.ToInt32(r["id"]);
                    this.Username = Convert.ToString(r["username"]);
                    if (r["card_id"] != DBNull.Value) this.CardId = Convert.ToInt32(r["card_id"]);
                    this.Telephone = Convert.ToString(r["telephone"]);
                    if (r["country_id"] != DBNull.Value) this.CountryId = Convert.ToInt32(r["country_id"]);
                    if (r["city_id"] != DBNull.Value) this.CityId = Convert.ToInt32(r["city_id"]);
                    this.Town = Convert.ToString(r["town"]);
                    this.Street = Convert.ToString(r["street"]);
                    this.Password = Convert.ToString(r["password"]);
                    this.name = Convert.ToString(r["name"]);

                    cmd.Connection.Close();


                }
            }
        }

        public Customer() { }

        //counstructor
        public Customer(int? id, string username, int? cardId, string telephone, int? countryId, int? cityId, string town, string street, string password,string name)
        {
            this.Id = id;
            this.Username = username;
            this.CardId = cardId;
            this.Telephone = telephone;
            this.CountryId = countryId;
            this.CityId = cityId;
            this.Town = town;
            this.Street = street;
            this.Password = password;
            this.name = name;

        }
        public int SaveData()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "SaveCustomerData";

                if (Username != null) cmd.Parameters.AddWithValue("username", Username);
                if (CardId != null) cmd.Parameters.AddWithValue("card_id", CardId);
                if (Telephone != null) cmd.Parameters.AddWithValue("telephone", Telephone);
                if (CountryId != null) cmd.Parameters.AddWithValue("country_id", CountryId);
                if (CityId != null) cmd.Parameters.AddWithValue("city_id", CityId);
                if (Town != null) cmd.Parameters.AddWithValue("town", Town);
                if (Street != null) cmd.Parameters.AddWithValue("street", Street);
                if (Password != null) cmd.Parameters.AddWithValue("password", Password);
                if (name != null) cmd.Parameters.AddWithValue("name", name);


                SqlParameter idParam = cmd.Parameters.Add("@id", SqlDbType.Int);
                idParam.Direction = ParameterDirection.InputOutput;

                SqlParameter resultParam = cmd.Parameters.Add("@result", SqlDbType.Int);
                resultParam.Direction = ParameterDirection.InputOutput;

                idParam.Value = this.Id;

                int c = cmd.ExecuteNonQuery();

                this.Id = Convert.ToInt32(idParam.Value);
                int result = Convert.ToInt32(resultParam.Value);
                cmd.Connection.Close();
                return result;

            }
        }

        //get using 
        public void  Delete()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "DeleteCustomer";

                cmd.Parameters.AddWithValue("@id", this.Id);

                cmd.ExecuteNonQuery();


            }
        }
        public static Customer[] GetCustomers(CustomerParameters parameters, out int rowsCount)
        {
            List<Customer> l = new List<Customer>();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "GetCustomers";
                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        Customer c = new Customer();
                        if (r["id"] != DBNull.Value) c.Id = Convert.ToInt32(r["id"]);
                        if (r["username"] != DBNull.Value) c.Username = Convert.ToString(r["username"]);
                        if (r["card_id"] != DBNull.Value) c.CardId = Convert.ToInt32(r["card_id"]);
                        if (r["telephone"] != DBNull.Value) c.Telephone = Convert.ToString(r["telephone"]);
                        if (r["country_id"] != DBNull.Value) c.CountryId = Convert.ToInt32(r["country_id"]);
                        if (r["city_id"] != DBNull.Value) c.CityId = Convert.ToInt32(r["city_id"]);
                        if (r["town"] != DBNull.Value) c.Town = Convert.ToString(r["town"]);
                        if (r["street"] != DBNull.Value) c.Street = Convert.ToString(r["street"]);
                        if (r["password"] != DBNull.Value) c.Password = Convert.ToString(r["password"]);
                        if (r["name"] != DBNull.Value) c.name = Convert.ToString(r["name"]);

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