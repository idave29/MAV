namespace MAV.Common.Models
{
    public class MaterialRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Label { set; get; }

        public string Brand { set; get; }

        public string Function { set; get; }
        public string ImageURL { set; get; }

        public string MaterialModel { set; get; }

        public string SerialNum { set; get; }

        public string Status { get; set; }

        public string MaterialType { set; get; }

        public string Owner { get; set; }
    }
}
