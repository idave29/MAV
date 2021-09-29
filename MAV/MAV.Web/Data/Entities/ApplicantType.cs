namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ApplicantType : IEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(30, ErrorMessage = "{0} must have maximun {1} characters")]
        [Display(Name = "Name")]
        public string Name { set; get; }
        public ICollection<Applicant> Applicants { get; set; }
    }
}
