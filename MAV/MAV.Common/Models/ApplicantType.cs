
namespace MAV.Common.Models
{
    using Newtonsoft.Json;

    public class ApplicantType
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("applicants")]
        public object Applicants { get; set; }
    }
}
