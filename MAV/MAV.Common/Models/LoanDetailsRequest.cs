namespace MAV.Common.Models
{
    using System;
    public class LoanDetailsRequest
    {
        public int Id { get; set; }

        public string Observations { set; get; }

        public DateTime DateTimeOut { set; get; }

        public DateTime DateTimeIn { set; get; }

        public MaterialRequest Material { get; set; }
    }
}
