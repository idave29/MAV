namespace MAV.Common.Models
{
    using Newtonsoft.Json;
    public class MaterialType
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("materials")]
        public object Materials { get; set; }


    }
}
