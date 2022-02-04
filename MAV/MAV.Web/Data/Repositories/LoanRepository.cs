namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class LoanRepository : GenericRepository<MAV.Web.Data.Entities.Loan>, ILoanRepository
    {
        private readonly DataContext dataContext;

        public LoanRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IQueryable GetLoanWithAplicantAndIntern()
        {
            return this.dataContext.Loans
                .Include(t => t.Applicant.User)
                .Include(t => t.Intern.User)
                //Cambiarle lo de collection para que tome el id
                .Include(t => t.LoanDetails);
        }

        public IQueryable GetLoan()
        {
            return this.dataContext.Loans
                .Include(t => t.Id);
        }
        public async Task<MAV.Web.Data.Entities.Applicant> GetApplicantLoansByEmail(EmailRequest emailRequest)
        {
            return await this.dataContext.Applicants
                .Include(a => a.User)
                .Include(a=> a.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.Status)
                .Include(a => a.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.MaterialType)
                .Include(a => a.Loans)
                .ThenInclude(l => l.Intern)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(l => l.User.Email.ToLower() == emailRequest.Email);
        }
    }
}
