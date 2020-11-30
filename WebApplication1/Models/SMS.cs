using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SMSParameters
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int To_number { get; set; }
        public DateTime Date { get; set; }
    }
    public class SMS
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int To_number { get; set; }
        public DateTime Date { get; set; }


        public SMS() { }

        public SMS(int id,string status,int to_number,DateTime date)
        {
            this.Id = id;
            this.Status = status;
            this.To_number = to_number;
            this.Date = date;
        }
    }
}