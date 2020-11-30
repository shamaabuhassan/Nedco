using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SMSParameters
    {
        public int ?SMS_Id { get; set; }
        public string Status { get; set; }
        public int ?To_number { get; set; }
        public DateTime Date { get; set; }
        public int ?Id { get; set; }
    }
    public class SMS
    {
        public int? SMS_Id { get; set; }
        public string Status { get; set; }
        public int ?To_number { get; set; }
        public DateTime Date { get; set; }
        public int? Id { get; set; }


        public SMS() { }

        public SMS(int? sms_id,string status,int ?to_number,DateTime date,int? id)
        {
            this.SMS_Id = sms_id;
            this.Status = status;
            this.To_number = to_number;
            this.Date = date;
            this.Id = id;
        }

        public int SaveData()
        {
            int result = 0;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "SaveSMS";


                if (SMS_Id != null) cmd.Parameters.AddWithValue("sms_id", SMS_Id);
                if (Status != null) cmd.Parameters.AddWithValue("status", Status);
                if (To_number != null) cmd.Parameters.AddWithValue("to_number", To_number);
                if (Date != null) cmd.Parameters.AddWithValue("date", Date);
                if (Id != null) cmd.Parameters.AddWithValue("id",Id);


                SqlParameter resultParam = cmd.Parameters.Add("@result", SqlDbType.Int);
                resultParam.Direction = ParameterDirection.InputOutput;

               

                int c = cmd.ExecuteNonQuery();

              
                result = Convert.ToInt32(resultParam.Value);
                cmd.Connection.Close();


            }
            return result;
        }
    }
}