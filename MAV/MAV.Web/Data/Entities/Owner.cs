namespace MAV.Web.Data.Entities
{
    public class Owner : IEntity
    {
        public int Id { get; set; }
        public User User { get; set; }

    }
}
