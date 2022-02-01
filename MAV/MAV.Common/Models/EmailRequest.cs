namespace MAV.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set;}
    }
}
