namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Status:IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(20, ErrorMessage = "{0} debe de tener máximo {1} caracteres")]
        [Display(Name = "Nombre del status")]
        public string Name { get; set; }

        public ICollection<Material> Materials { get; set; }
        public ICollection<LoanDetail> LoanDetails { get; set; }
    }
}
