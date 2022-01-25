namespace MAV.Common.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Applicant
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("applicantType")]
        public ApplicantType ApplicantType { get; set; }
        //public string ApplicantType;

        [JsonProperty("loans")]
        public ICollection<Loan> Loans { get; set; }
    }
}
