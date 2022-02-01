namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
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
        public IQueryable GetApplicantLoansByEmail(EmailRequest emailRequest)
        {
            return this.dataContext.Loans
                .Include(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.Status)
                .Include(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.MaterialType)
                .Include(l => l.Intern)
                .ThenInclude(a => a.User)
                .Include(l => l.Applicant)
                .ThenInclude(a => a.User)
                .Where(l => l.Applicant.User.Email.ToLower() == emailRequest.Email);
        }
    }
}
