using System;
using System.Collections.Generic;
using System.Text;

namespace MAV.Common.Models
{
    public class OwnerResponse
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }
        public string OldPassword { get; set; }

        //public string FullName == ("{FirstName} {LastName}";
    }
}
