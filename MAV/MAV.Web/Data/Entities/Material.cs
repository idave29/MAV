namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Material : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(30, ErrorMessage = "{0} must have maximun {1} characters")]
        [Display(Name = "Name")]
        public string Name { set; get; }

        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(6, ErrorMessage = "{0} must have maximun {1} characters")]
        [Display(Name = "Label")]
        public string Label { set; get; }

        [MaxLength(50, ErrorMessage = "{0} must have maximun {1} characters")]
        [Display(Name = "Brand")]
        [Required(ErrorMessage = "{0} is required")]
        public string Brand { set; get; }

        [MaxLength(15, ErrorMessage = "{0} must have maximun {1} characters")]
        [Display(Name = "Model")]
        [Required(ErrorMessage = "{0} is required")]
        public string MaterialModel { set; get; }

        [MaxLength(15, ErrorMessage = "{0} must have maximun {1} characters")]
        [Display(Name = "Serial Number")]
        [Required(ErrorMessage = "{0} is required")]
        public string SerialNum { set; get; }

        public Status Status { get; set; }

        public Owner Owner { get; set; }

        public ICollection<LoanDetail> LoanDetails { set; get; }
    }
}
