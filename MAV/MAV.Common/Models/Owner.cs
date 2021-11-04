namespace MAV.Common.Models
{
    using Newtonsoft.Json;
    public class Owner
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("materials")]
        public object Materials { get; set; }
    }
}
