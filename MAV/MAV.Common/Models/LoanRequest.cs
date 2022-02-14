namespace MAV.Common.Models
{
    using System.Collections.Generic;
    public class LoanRequest
    {
        public int Id { get; set; }

        public InternRequest Intern { get; set; }

        public ICollection<LoanDetailsRequest> LoanDetails { get; set; }
    }
}
