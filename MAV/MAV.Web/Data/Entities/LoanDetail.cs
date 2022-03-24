namespace MAV.Web.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class LoanDetail:IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Observaciones")]
        public string Observations { get; set; }

        [Display(Name = "Fecha y tiempo del préstamo")]
        public DateTime DateTimeOut { get; set; }

        [Display(Name = "Fecha y tiempo de la devolución")]
        public DateTime DateTimeIn { get; set; }

        [Display(Name = "Material")]
        public Material Material { get; set; }

        [Display(Name = "Estado")]
        public Status Status { get; set; }

        [Display(Name = "Préstamo")]
        public Loan Loan { get; set; }
    }
}
