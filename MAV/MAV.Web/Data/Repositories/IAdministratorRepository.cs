namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;

    interface IAdministratorRepository : IGenericRepository<Administrator>
    {
        IEnumerable<SelectListItem> GetComboAdministrators();
        IQueryable GetAdministratorsWithUser();
    }
}
