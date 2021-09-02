namespace MAV.Web.Data.Entities
{
    public class Applicant : IEntity
    {
        public int Id { get; set; }
        public User User { get; set; }
    }
}
