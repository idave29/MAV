namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    public class LoanRepository : GenericRepository<Loan>, ILoanRepository
    {
        private readonly DataContext dataContext;

        public LoanRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IQueryable GetLoan()
        {
            return this.dataContext.Loans
                .Include(t => t.Id);
        }
    }
}
