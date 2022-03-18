namespace MAV.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [EmailAddress]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MinLength(6, ErrorMessage = "{0} debe de tener minimo {1} caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
