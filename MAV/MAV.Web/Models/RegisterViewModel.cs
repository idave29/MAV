using MAV.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAV.Web.Models
{
    public class RegisterViewModel : User
    {
        [Required(ErrorMessage = "{0} es obligatorio")]
        [MinLength(6, ErrorMessage = "{0} debe de tener mínimo {1} caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [Compare("Password")]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [Display(Name = "Rol")]
        public string RoleName { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe de seleccionar un tipo")]
        [Display(Name = "Tipo")]
        public int TypeId { get; set; }

        public IEnumerable<SelectListItem> Types { get; set; }

    }
}
