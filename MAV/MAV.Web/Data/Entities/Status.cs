using System.Collections.Generic;

namespace MAV.Web.Data.Entities
{
    public class Status:IEntity
    {
        public int Id { get; set; }

        public ICollection<LoanDetail> LoanDetails { set; get; }

        public ICollection<Material> Materials { set; get; }
    }
}
