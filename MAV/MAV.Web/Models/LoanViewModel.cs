using MAV.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAV.Web.Models
{
    public class LoanViewModel : Loan
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Tienes que seleccionar un solicitante")]
        [Display(Name = "Solicitante")]
        public int ApplicantId { get; set; }

        public IEnumerable<SelectListItem> Applicants { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Tienes que seleccionar un material")]
        [Display(Name = "Material")]
        public int MaterialId { get; set; }

        public IEnumerable<SelectListItem> Materials { get; set; }

    }
}
