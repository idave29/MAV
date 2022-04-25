using System;
using System.Collections.Generic;
using System.Text;

namespace MAV.Common.Models
{
    public class MaterialResponse
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
        public bool Deleted { get; set; }

        public string ImageFullPath { get; set; }

        public byte[] ImageArray { get; set; }

    }
}
