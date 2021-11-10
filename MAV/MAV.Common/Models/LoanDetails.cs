namespace MAV.Common.Models
{
    using Newtonsoft.Json;
    using System;

    public class LoanDetails
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("observations")]
        public object Observations { get; set; }

        [JsonProperty("dateTimeOut")]
        public DateTime DateTimeOut { get; set; }

        [JsonProperty("dateTimeIn")]
        public DateTime DateTimeIn { get; set; }

        [JsonProperty("material")]
        public object Material { get; set; }

        [JsonProperty("loan")]
        public object Loan { get; set; }
    }
}
