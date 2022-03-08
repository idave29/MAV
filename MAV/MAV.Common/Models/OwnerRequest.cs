using System;
using System.Collections.Generic;
using System.Text;

namespace MAV.Common.Models
{
    public class OwnerRequest
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }    
        public ICollection<MaterialRequest> Materials { get; set; }
    }
}
