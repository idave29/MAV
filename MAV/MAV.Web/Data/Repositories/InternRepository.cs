namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class InternRepository : GenericRepository<MAV.Web.Data.Entities.Intern>, IInternRepository
    {
        private readonly DataContext dataContext;

        public InternRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<MAV.Web.Data.Entities.Intern> GetByIdInternWithLoansAsync(int id)
        {
            return await this.dataContext.Interns
                .Include(t => t.User)
                .Include(t => t.Loans)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<MAV.Web.Data.Entities.Intern> GetByIdInternWithLoansLoansDetailsAsync(int id)
        {
            return await this.dataContext.Interns
                .Include(t => t.User)
                .Include(t => t.Loans)
                .ThenInclude(c => c.LoanDetails)
                .ThenInclude(cd => cd.Material)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<MAV.Web.Data.Entities.Intern> GetByIdWithUserAsync(int id)
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

        public MAV.Common.Models.InternRequest GetInternsWithUser()
        {
            var a = this.dataContext.Interns
                .Include(a => a.User)
                .FirstOrDefault();

            if (a == null)
            {
                return null;
            }

            var x = new InternRequest
            {
                Id = a.Id,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                Email = a.User.Email,
                PhoneNumber = a.User.PhoneNumber

            };
            return x;
        }
    }
}
