namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using System.Linq;
    public interface ILoanDetailRepository : IGenericRepository<LoanDetail>
    {
        //IEnumerable<SelectListItem> GetComboMaterial();

        IQueryable GetLoanDetails();
    }
}
