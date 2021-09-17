namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    public class Material : IEntity
    {
        public int Id { get; set; }

        public Status Status { get; set; }

        public ICollection<LoanDetail> LoanDetails { set; get; }
    }
}
