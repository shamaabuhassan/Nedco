using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
 
    public class TransferParameters{
        public int? Id { get; set; }
        public string SenderOTP { get; set; }
        public int? MeterId { get; set; }
        public decimal? Amount { get; set; }

      
    }
public class Transfer
    {
        public string Status { get; set; }
        public int? Id { get; set; }
        public string SenderOTP { get; set; }
        public int? MeterId { get; set; }
        public decimal? Amount { get; set; }

 

        public Transfer()
        {

        }

     public Transfer(int id)
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
                    if (r["id"] != DBNull.Value) this.Id = Convert.ToInt32(r["id"]);
                    this.SenderOTP = Convert.ToString(r["senderOTP"]);
                    if (r["meter_id"] != DBNull.Value) this.MeterId = Convert.ToInt32(r["meter_id"]);
                    if (r["amount"] != DBNull.Value) this.Amount = Convert.ToDecimal(r["amount"]);
                    cmd.Connection.Close();


                }
            }
        }


        //constructor

        public Transfer(int? id, string senderOTP, int? meterId, decimal? amount)
        {
            this.Id = id;
            this.SenderOTP = senderOTP;
            this.MeterId = meterId;
            this.Amount = amount;

        }

        public int RandomNumber(int digits)
        {
            int count = 0;
            int rand = 0;
            while (count != 1)
            {
                Random random = new Random();
                rand = random.Next();
                int len = rand.ToString().Length;

                if (len == digits)
                    count = 1;
            }
            return rand;
        }

        public void Delete()
        {
      using(SqlCommand cmd=new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "DeleteTransfer";

                cmd.Parameters.AddWithValue("@id", this.Id);

                cmd.ExecuteNonQuery();
            }
        }

        public int SaveData()
        {
            int result = 0;
            Topup topup = new Topup(SenderOTP);
            if (topup.Status == "0")

            {
                if (topup.Amount > Amount)
                {

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = new SqlConnection(cstr.con);
                        cmd.Connection.Open();
                        cmd.CommandText = "SaveTransferData";


                        if (SenderOTP != null) cmd.Parameters.AddWithValue("senderOTP", SenderOTP);
                        if (MeterId != null) cmd.Parameters.AddWithValue("meter_id", MeterId);
                        if (Amount != null) cmd.Parameters.AddWithValue("amount", Amount);

                        SqlParameter idParam = cmd.Parameters.Add("@id", SqlDbType.Int);
                        idParam.Direction = ParameterDirection.InputOutput;

                        SqlParameter resultParam = cmd.Parameters.Add("@result", SqlDbType.Int);
                        resultParam.Direction = ParameterDirection.InputOutput;

                        idParam.Value = this.Id;

                        int c = cmd.ExecuteNonQuery();

                        this.Id = Convert.ToInt32(idParam.Value);
                        result = Convert.ToInt32(resultParam.Value);
                        cmd.Connection.Close();

                        decimal? amount = topup.Amount - Amount;
                        Topup topup1 = new Topup(null,topup.MeterId, amount, topup.CardId);
                        topup1.SaveData();
                        Topup topup2 = new Topup(null,MeterId, Amount, topup.CardId);
                        topup2.SaveData();
                        topup.Delete();
                    }
                }
            }
            return result;
        }

       

        public static Transfer[] GetTransfers(TransferParameters parameters, out int rowsCount)
        {
            List<Transfer> l = new List<Transfer>();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(cstr.con);
                cmd.Connection.Open();
                cmd.CommandText = "GetTransfers";

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        Transfer c = new Transfer();
                        if (r["id"] != DBNull.Value) c.Id = Convert.ToInt32(r["id"]);
                        if (r["senderOTP"] != DBNull.Value) c.SenderOTP = Convert.ToString(r["senderOTP"]);
                        if (r["meter_id"] != DBNull.Value) c.MeterId = Convert.ToInt32(r["meter_id"]);
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