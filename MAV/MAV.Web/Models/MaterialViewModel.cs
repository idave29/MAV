using MAV.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAV.Web.Models
{
    public class MaterialViewModel : Material
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Tienes que seleccionar un status")]
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Tienes que seleccionar un responsable")]
        [Display(Name = "Responsable")]
        public int OwnerId { get; set; }
        public IEnumerable<SelectListItem> Applicants { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Tienes que seleccionar un tipo de material")]
        [Display(Name = "Tipo de Material")]
        public int MaterialTypeId { get; set; }
        public IEnumerable<SelectListItem> MaterialTypes { get; set; }


    }
}
