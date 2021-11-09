namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        public IQueryable GetMaterialsWithTypeAndStatus()
        {
            return this.dataContext.Materials
                .Include(t => t.MaterialType)
                .Include(t => t.Status)
                .Include(t => t.Owner.User);
        }

        public IQueryable GetMaterials()
        {
            return this.dataContext.Materials
                .Include(t => t.Id)
                .Include(t => t.Name)
                .Include(t => t.Label);
        }

    }
}
