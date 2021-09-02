namespace MAV.Web.Data.Entities
{
    public class Administrator : IEntity
    {
        public int Id { get; set; }
        public User User { get; set; }
    }
}
