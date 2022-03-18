namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Loan:IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Aplicante")]
        public Applicant Applicant { get; set; }
        [Display(Name = "Becario")]
        public Intern Intern { get; set; }
        [Display(Name = "Detalles del préstamo")]
        public ICollection<LoanDetail> LoanDetails { get;set; }

    }
}
