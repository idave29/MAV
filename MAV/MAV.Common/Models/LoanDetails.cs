namespace MAV.Common.Models
{
    using Newtonsoft.Json;
    using System;

    public class LoanDetails
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("observations")]
        public string Observations { get; set; }

        [JsonProperty("dateTimeOut")]
        public DateTime DateTimeOut { get; set; }

        [JsonProperty("dateTimeIn")]
        public DateTime DateTimeIn { get; set; }

        [JsonProperty("material")]
        public Material Material { get; set; }

        //[JsonProperty("loan")]
        //public object Loan { get; set; }
    }
}
