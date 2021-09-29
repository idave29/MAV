namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApplicantRepository : GenericRepository<Applicant>, IApplicantRepository
    {
        private readonly DataContext dataContext;

        public ApplicantRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<Applicant> GetByIdApplicantWithLoansAsync(int id)
        {
            return await this.dataContext.Applicants
                .Include(t => t.User)
                .Include(t => t.Loans)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Applicant> GetByIdApplicantWithLoansLoanDetailsAsync(int id)
        {
            return await this.dataContext.Applicants
                .Include(t => t.User)
                .Include(t => t.Loans)
                .ThenInclude(c => c.LoanDetails)
                .ThenInclude(cd => cd.Material)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public IEnumerable<SelectListItem> GetComboApplicants()
        {
            var list = this.dataContext.Applicants.Select(t => new SelectListItem
            {
                Text = t.User.FullName,
                Value = $"{t.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un solicitante...)",
                Value = "0"
            });
            return list;
        }

        public IQueryable GetApplicantsWithUser()
        {
            return this.dataContext.Applicants
                .Include(t => t.User);
        }
    }
}
