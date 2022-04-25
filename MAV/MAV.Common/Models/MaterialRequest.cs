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

        public int Status { get; set; }
        public bool Deleted { get; set; }

        public int MaterialType { set; get; }

        public int Owner { get; set; }

        public string ImageFullPath { get; set; }

        public byte[] ImageArray { get; set; }
    }
}
