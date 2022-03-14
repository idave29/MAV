namespace MAV.Common.Models
{
    using System.Collections.Generic;
    public class LoanRequest
    {
        public int Id { get; set; }

        public string Intern { get; set; }
        public string Applicant { get; set; }

        //public ICollection<LoanDetailsRequest> LoanDetails { get; set; }
    }
}
