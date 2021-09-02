namespace MAV.Web.Data.Entities
{
    public class Intern : IEntity
    {
        public int Id { get; set; }
        public User User { get; set; }
    }
}
