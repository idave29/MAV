namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    public class Loan:IEntity
    {
        public int Id { get; set; }
        public Applicant Applicant { get; set; }
        public Intern Intern { get; set; }
        //public LoanDetail LoanDetails { get; set; }
        public ICollection<LoanDetail> LoanDetails { get;set; }

    }
}
