namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    public class MaterialRepository : GenericRepository<Material>, IMaterialRepository
    {
        private readonly DataContext dataContext;

        public MaterialRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboMaterial()
        {
            var list = this.dataContext.Materials.Select(t => new SelectListItem
            {
                Text = t.Label,
                Value = $"{t.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un material...)",
                Value = "0"
            });
            return list;
        }

        public IQueryable GetMaterialsWithOwner()
        {
            return this.dataContext.Materials
                .Include(t => t.Owner);
        }
    }
}
