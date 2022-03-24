namespace MAV.Web.Models
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class AdministratorViewModel:Administrator
    {

        [Required(ErrorMessage = "{0} es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe de ecoger un usuario")]
        [Display(Name = "Usuario")]
        public string UserUserName { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }

    }
}
