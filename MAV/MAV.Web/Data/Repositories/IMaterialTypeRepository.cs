namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    public interface IMaterialTypeRepository : IGenericRepository<MaterialType>
    {
        IEnumerable<SelectListItem> GetComboMateriaType();

        MAV.Common.Models.MaterialTypeRequest GetMaterialTypes();
    }
}
