using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAV.Web.Data.Entities
{
    public class Intern : IEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        public User User { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}
