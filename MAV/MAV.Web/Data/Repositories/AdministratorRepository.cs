namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    public class AdministratorRepository : GenericRepository<Administrator>, IAdministratorRepository
    {
        private readonly DataContext dataContext;

        public AdministratorRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboAdministrators()
        {
            var list = this.dataContext.Administrators.Select(t => new SelectListItem
            {
                Text = t.User.FullName,
                Value = $"{t.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un administrador...)",
                Value = "0"
            });
            return list;
        }

        public IQueryable GetAdministratorsWithUser()
        {
            return this.dataContext.Administrators
                .Include(t => t.User);
        }
    }
}
