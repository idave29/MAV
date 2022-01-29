using System;
using System.Collections.Generic;
using System.Text;

namespace MAV.Common.Models
{
    public class LoanDetailsResponse
    {
        public int Id { get; set; }
        public DateTime DateTimeOut { set; get; }
        public string Material { get; set; }
        public User User { get; set; }
    }
}
