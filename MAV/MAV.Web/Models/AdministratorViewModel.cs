namespace MAV.Web.Models
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class AdministratorViewModel:Administrator
    {

        [Required(ErrorMessage = "{0} is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You have to select an User")]
        [Display(Name = "User")]
        public string UserUserName { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }

    }
}
