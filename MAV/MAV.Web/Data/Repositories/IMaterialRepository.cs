namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;

    public interface IMaterialRepository : IGenericRepository<Material>
    {
        IEnumerable<SelectListItem> GetComboMaterial();

        IQueryable GetMaterialsWithOwner();
    }
}
