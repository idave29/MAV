namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using System.Linq;

    public interface ILoanRepository : IGenericRepository<Loan>
    {
        IQueryable GetLoan();
    }
}
