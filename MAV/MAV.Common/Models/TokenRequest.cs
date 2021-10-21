namespace MAV.Common.Models
{
    using Newtonsoft.Json;
    public class TokenRequest
    {
        [JsonProperty("token")]
        public string Username { get; set; }

        [JsonProperty("expiration")]
        public string Password { get; set; }
    }
}
