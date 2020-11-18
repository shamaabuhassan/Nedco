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
        public int? MeterId { get; set; }
        public int? CardId { get; set; }
        public string Telephone { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public string Password { get; set; }
        public string name { get; set; }

    }
    public class Customer
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public int? MeterId { get; set; }
        public int? CardId { get; set; }
        public string Telephone { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public string Password { get; set; }
        public string name { get; set; }
        public Customer(int ID)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "GetCustomerByID";
                cmd.Parameters.AddWithValue("@ID", Id);

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    r.Read();
                    if (r["id"] != DBNull.Value) this.Id = Convert.ToInt32(r["id"]);
                    this.Username = Convert.ToString(r["username"]);
                    if (r["meter_id"] != DBNull.Value) this.MeterId = Convert.ToInt32(r["meter_id"]);
                    if (r["card_id"] != DBNull.Value) this.CardId = Convert.ToInt32(r["card_id"]);
                    this.Telephone = Convert.ToString(r["telephone"]);
                    if (r["country_id"] != DBNull.Value) this.CountryId = Convert.ToInt32(r["country_id"]);
                    if (r["city_id"] != DBNull.Value) this.CityId = Convert.ToInt32(r["city_id"]);
                    this.Area = Convert.ToString(r["area"]);
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
                    if (r["meter_id"] != DBNull.Value) this.MeterId = Convert.ToInt32(r["meter_id"]);
                    if (r["card_id"] != DBNull.Value) this.CardId = Convert.ToInt32(r["card_id"]);
                    this.Telephone = Convert.ToString(r["telephone"]);
                    if (r["country_id"] != DBNull.Value) this.CountryId = Convert.ToInt32(r["country_id"]);
                    if (r["city_id"] != DBNull.Value) this.CityId = Convert.ToInt32(r["city_id"]);
                    this.Area = Convert.ToString(r["area"]);
                    this.Street = Convert.ToString(r["street"]);
                    this.Password = Convert.ToString(r["password"]);
                    this.name = Convert.ToString(r["name"]);

                    cmd.Connection.Close();


                }
            }
        }

        public Customer() { }

        //counstructor
        public Customer(int? id, string username, int? meterId, int? cardId, string telephone, int? countryId, int? cityId, string area, string street, string password,string name)
        {
            this.Id = id;
            this.Username = username;
            this.MeterId = meterId;
            this.CardId = cardId;
            this.Telephone = telephone;
            this.CountryId = countryId;
            this.CityId = cityId;
            this.Area = area;
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
                if (MeterId != null) cmd.Parameters.AddWithValue("meter_id", MeterId);
                if (CardId != null) cmd.Parameters.AddWithValue("card_id", CardId);
                if (Telephone != null) cmd.Parameters.AddWithValue("telephone", Telephone);
                if (CountryId != null) cmd.Parameters.AddWithValue("country_id", CountryId);
                if (CityId != null) cmd.Parameters.AddWithValue("city_id", CityId);
                if (Area != null) cmd.Parameters.AddWithValue("area", Area);
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

        //get using list
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
                        if (r["meter_id"] != DBNull.Value) c.MeterId = Convert.ToInt32(r["meter_id"]);
                        if (r["card_id"] != DBNull.Value) c.CardId = Convert.ToInt32(r["card_id"]);
                        if (r["telephone"] != DBNull.Value) c.Telephone = Convert.ToString(r["telephone"]);
                        if (r["country_id"] != DBNull.Value) c.CountryId = Convert.ToInt32(r["country_id"]);
                        if (r["city_id"] != DBNull.Value) c.CityId = Convert.ToInt32(r["city_id"]);
                        if (r["area"] != DBNull.Value) c.Area = Convert.ToString(r["area"]);
                        if (r["street"] != DBNull.Value) c.Street = Convert.ToString(r["street"]);
                        if (r["password"] != DBNull.Value) c.Password = Convert.ToString(r["password"]);
                        if (r["name"] != DBNull.Value) c.Username = Convert.ToString(r["name"]);

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