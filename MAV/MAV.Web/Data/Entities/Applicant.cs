namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Applicant : IEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        public bool Debtor { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        public User User { get; set; }
        public ApplicantType ApplicantType { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}
