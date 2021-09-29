using MAV.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAV.Web.Data.Repositories
{
    public class MaterialTypeRepository : GenericRepository<MaterialType>, IMaterialTypeRepository
    {
        private readonly DataContext dataContext;

        public MaterialTypeRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboMateriaType()
        {
            var list = this.dataContext.MaterialTypes.Select(st => new SelectListItem
            {
                Text = st.Name,
                Value = $"{st.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un Tipo Materia...)",
                Value = "0"
            });
            return list;
        }
    }
}
