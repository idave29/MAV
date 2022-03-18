using MAV.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace MAV.Web.Models
{
    public class ChangePasswordViewModel : User
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [MinLength(6, ErrorMessage = "{0} debe de tener mínimo {1} caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Password viejo")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MinLength(6, ErrorMessage = "{0} debe de tener mínimo {1} caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [Compare("Password")]
        [Display(Name = "Confirmar password")]
        public string ConfirmPassword { get; set; }
    }
}
