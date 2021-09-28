namespace MAV.Web.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class LoanDetail:IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Observations")]
        public string Observations { set; get; }

        [Display(Name = "Date and time of loan")]
        public DateTime DateTimeOut { set; get; }

        [Display(Name = "Date and time of return")]
        public DateTime DateTimeIn { set; get; }

        public Material Material { get; set; }
        public Loan Loan { get; set; }
    }
}
