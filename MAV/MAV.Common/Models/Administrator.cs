namespace MAV.Common.Models
{
    using Newtonsoft.Json;
    public class Administrator
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}
