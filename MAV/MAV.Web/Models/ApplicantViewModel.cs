using MAV.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAV.Web.Models
{
    public class ApplicantViewModel : Applicant
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Tienes que seleccionar un tipo")]
        [Display(Name = "Tipo")]
        public int TypeId { get; set; }

        public IEnumerable<SelectListItem> Types { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Tienes que seleccionar un usuario")]
        [Display(Name = "Usuario")]
        public string UserUserName { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
