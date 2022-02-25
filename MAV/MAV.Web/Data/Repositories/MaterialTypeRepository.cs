namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;

    public class MaterialTypeRepository : GenericRepository<MAV.Web.Data.Entities.MaterialType>, IMaterialTypeRepository
    {
        private readonly DataContext dataContext;

        public MaterialTypeRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboMateriaType()
        {
            var list = this.dataContext.MaterialTypes.Select(mt => new SelectListItem
            {
                Text = mt.Name,
                Value = $"{mt.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Select an Material Type...)",
                Value = "0"
            });
            return list;
        }
        
        public IQueryable GetMaterialTypes()
        {
            return this.dataContext.MaterialTypes;
        }

        public MaterialTypeRequest GetMaterialTypeById(int id)
        {
            var a = this.dataContext.MaterialTypes
                .FirstOrDefault(m => m.Id == id);

            if (a == null)
            {
                return null;
            }

            var x = new MaterialTypeRequest
            {
                Id = a.Id,
                Name = a.Name
            };

            return x;
        }
    
        public IEnumerable<MaterialTypeRequest> GetMaterialTypesByName(string name)
        {
            var a = this.dataContext.MaterialTypes
                .Where(mt => mt.Name == name);

            if (a == null)
            {
                return null;
            }

            var x = a.Select(mt => new MaterialTypeRequest
            {
               Id = mt.Id,
               Name = mt.Name

            }).ToList();
            
            return x;
        }
    }
}
