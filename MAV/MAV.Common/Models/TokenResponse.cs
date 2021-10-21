using System;
using System.Collections.Generic;
using System.Text;

namespace MAV.Common.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
