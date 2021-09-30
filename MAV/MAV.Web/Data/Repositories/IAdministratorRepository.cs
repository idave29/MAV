namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAdministratorRepository : IGenericRepository<Administrator>
    {
        IEnumerable<SelectListItem> GetComboAdministrators();
        IQueryable GetAdministratorsWithUser();

        Task<Administrator> GetByIdWithUserAsync(int id);
    }
}
