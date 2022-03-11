namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Status:IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(20, ErrorMessage = "{0} must have maximun {1} characters")]
        [Display(Name = "Status name")]

        public string Name { get; set; }

        //public ICollection<Material> Materials { set; get; }
    }
}
