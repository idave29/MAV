namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMaterialTypeRepository : IGenericRepository<MaterialType>
    {
        IEnumerable<SelectListItem> GetComboMateriaType();
        IQueryable GetMaterialTypes();
        MaterialTypeRequest GetMaterialTypeById(int id);
        MaterialType GetMaterialTypesByName(string name);
        Task<MaterialType> GetByIdMaterialTypeAsync(int id);
    }
}
