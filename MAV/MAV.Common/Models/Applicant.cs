namespace MAV.Common.Models
{
    using Newtonsoft.Json;
    public class Applicant
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("applicantType")]
        public object ApplicantType { get; set; }

        [JsonProperty("loans")]
        public object Loans { get; set; }
    }
}
