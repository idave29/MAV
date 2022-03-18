using MAV.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAV.Web.Models
{
    public class LoanDetailViewModel : LoanDetail
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Tienes que seleccionar un status")]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public IEnumerable<SelectListItem> Statuses { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Tienes que seleccionar un material")]
        [Display(Name = "Material")]
        public int MaterialId { get; set; }

        public IEnumerable<SelectListItem> Materials { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Tienes que seleccionar un préstamo")]
        [Display(Name = "Préstamo")]
        public int LoanID { get; set; }
    }
}
