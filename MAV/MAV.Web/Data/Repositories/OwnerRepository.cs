namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class OwnerRepository : GenericRepository<MAV.Web.Data.Entities.Owner>, IOwnerRepository
    {
        private readonly DataContext dataContext;

        public OwnerRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<MAV.Web.Data.Entities.Owner> GetByIdOwnerWithMaterialsAsync(int id)
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

        public MAV.Common.Models.OwnerRequest GetOwnersWithUser()
        {
            var a = this.dataContext.Owners
                .Include(a => a.User)
                .Include(m => m.Materials)
                .FirstOrDefault();

            if (a == null)
            {
                return null;
            }
            var x = new OwnerRequest
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
