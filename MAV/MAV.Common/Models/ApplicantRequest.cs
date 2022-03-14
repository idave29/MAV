namespace MAV.Common.Models
{
    using System.Collections.Generic;
    public class ApplicantRequest
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ApplicantType { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }

        //public ICollection<LoanRequest> Loans { get; set; }
    }
}
