using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAV.Common.Models
{
    public class TokenResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expiration")]
        public DateTime Expiration { get; set; }
    }
}
