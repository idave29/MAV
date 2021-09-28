namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    public class Applicant : IEntity
    {
        public int Id { get; set; }
        public User User { get; set; }
        public ApplicantType ApplicantType { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}
