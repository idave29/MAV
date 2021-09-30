namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class InternRepository : GenericRepository<Intern>, IInternRepository
    {
        private readonly DataContext dataContext;

        public InternRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<Intern> GetByIdInternWithLoansAsync(int id)
        {
            return await this.dataContext.Interns
                .Include(t => t.User)
                .Include(t => t.Loans)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Intern> GetByIdInternWithLoansLoansDetailsAsync(int id)
        {
            return await this.dataContext.Interns
                .Include(t => t.User)
                .Include(t => t.Loans)
                .ThenInclude(c => c.LoanDetails)
                .ThenInclude(cd => cd.Material)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<Intern> GetByIdWithUserAsync(int id)
        {
            return this.dataContext.Interns
                 .Include(t => t.User)
                 .FirstOrDefaultAsync(e => e.Id == id);
        }

        public IEnumerable<SelectListItem> GetComboInterns()
        {
            var list = this.dataContext.Interns.Select(t => new SelectListItem
            {
                Text = t.User.FullName,
                Value = $"{t.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un becario...)",
                Value = "0"
            });
            return list;
        }

        public IQueryable GetInternsWithUser()
        {
            return this.dataContext.Interns
                .Include(t => t.User);
        }
    }
}
