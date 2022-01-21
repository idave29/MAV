namespace MAV.Common.Models
{
    using Newtonsoft.Json;
    public class Status
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{this.Id}.- {this.Name}";
        }

    }
}
