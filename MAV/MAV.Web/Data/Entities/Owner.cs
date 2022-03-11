namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    public class Owner : IEntity
    {
        public int Id { get; set; }
        public User User { get; set; }
        //public ICollection<Material> Materials { set; get; }
    }
}
