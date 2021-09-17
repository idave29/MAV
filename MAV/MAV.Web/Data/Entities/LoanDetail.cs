namespace MAV.Web.Data.Entities
{
    public class LoanDetail:IEntity
    {
        public int Id { get; set; }

        public Material Material { get; set; }

        public Status Status { get; set; }

    }
}
