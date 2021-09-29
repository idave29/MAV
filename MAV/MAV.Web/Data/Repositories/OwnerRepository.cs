namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        private readonly DataContext dataContext;

        public OwnerRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<Owner> GetByIdOwnerWithMaterialsAsync(int id)
        {
            return await this.dataContext.Owners
                .Include(t => t.User)
                .Include(t => t.Materials)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public IEnumerable<SelectListItem> GetComboOwners()
        {
            var list = this.dataContext.Owners.Select(t => new SelectListItem
            {
                Text = t.User.FullName,
                Value = $"{t.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un propietario...)",
                Value = "0"
            });
            return list;
        }

        public IQueryable GetOwnersWithUser()
        {
            return this.dataContext.Owners
                .Include(t => t.User);
        }
    }
}
