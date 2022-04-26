namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Material : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Foto")]
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(20, ErrorMessage = "{0} debe de tener máximo {1} caracteres")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(6, ErrorMessage = "{0} debe de tener máximo {1} caracteres")]
        [Display(Name = "Etiqueta")]
        public string Label { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(30, ErrorMessage = "{0} debe de tener máximo {1} caracteres")]
        [Display(Name = "Marca")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(15, ErrorMessage = "{0} debe de tener máximo {1} caracteres")]
        [Display(Name = "Modelo")]
        public string MaterialModel { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(15, ErrorMessage = "{0} debe de tener máximo {1} caracteres")]
        [Display(Name = "Número de serie")]
        public string SerialNum { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(100, ErrorMessage = "{0} debe de tener máximo {1} caracteres")]
        [Display(Name = "Función")]
        public string Function { get; set; }

        public bool Deleted { get; set; }

        public Status Status { get; set; }

        public Owner Owner { get; set; }

        public ICollection<LoanDetail> LoanDetails { get; set; }

        public MaterialType MaterialType { get; set; }

        public string ImageFullPath => string.IsNullOrEmpty(ImageURL)
            ? null
            : $"https://mediosaudiovisualesumad.azurewebsites.net{ImageURL.Substring(1)}";
    }
}
