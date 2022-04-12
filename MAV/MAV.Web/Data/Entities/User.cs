namespace MAV.Web.Data.Entities
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser
    {
        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Display(Name = "Número de celular")]
        public override string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        public override string Email { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName => $"{LastName} {FirstName}";

        public bool Deleted { get; set; }
    }
}
