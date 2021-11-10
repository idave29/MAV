namespace MAV.Common.Models
{
    using Newtonsoft.Json;
    public class Material
    {
        public class Root
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("brand")]
            public string Brand { get; set; }

            [JsonProperty("materialModel")]
            public string MaterialModel { get; set; }

            [JsonProperty("serialNum")]
            public string SerialNum { get; set; }

            [JsonProperty("status")]
            public object Status { get; set; }

            [JsonProperty("owner")]
            public object Owner { get; set; }

            [JsonProperty("loanDetails")]
            public object LoanDetails { get; set; }

            [JsonProperty("materialType")]
            public object MaterialType { get; set; }
        }
    }
}
