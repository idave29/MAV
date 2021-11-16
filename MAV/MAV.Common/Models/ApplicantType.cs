
namespace MAV.Common.Models
{
    using Newtonsoft.Json;

    public class ApplicantType
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{this.Id} {this.Name}";
        }

        [JsonProperty("applicants")]
        public object Applicants { get; set; }
    }
}
