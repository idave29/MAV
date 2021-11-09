using MAV.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MAV.Web.Data.Repositories
{
    public class LoanDetailRepository : GenericRepository<LoanDetail>, ILoanDetailRepository
    {
        private readonly DataContext dataContext;

        public LoanDetailRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IQueryable GetLoanDetails()
        {
            return this.dataContext.LoanDetails
                .Include(t => t.Id)
                .Include(t => t.Loan)
                .Include(t => t.Material);
        }
    }
}
