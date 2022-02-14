namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MaterialRepository : GenericRepository<MAV.Web.Data.Entities.Material>, IMaterialRepository
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

        public MAV.Common.Models.MaterialRequest GetMaterials()
        {
            var a = this.dataContext.Materials
                .Include(s => s.Status)
                .Include(o => o.Owner)
                .Include(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.MaterialType)
                .FirstOrDefault();
        
            if (a == null)
            {
                return null;
            } 

            var x = new MaterialRequest
            {
                Id = a.Id,
                Name = a.Name,
                Label = a.Label,
                Brand = a.Brand,
                MaterialModel = a.MaterialModel,
                SerialNum = a.SerialNum,
                Status = a.Status.Name,
                MaterialType = a.MaterialType.Name
                
            };
            return x;

        }
    }
}
