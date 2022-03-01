namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAdministratorRepository : IGenericRepository<Administrator>
    {
        IEnumerable<SelectListItem> GetComboAdministrators();

        IEnumerable<AdministratorRequest> GetAdministrators();

        IQueryable GetAdministratorsWithUser();
        Task<Administrator> GetByIdWithUserAsync(int id);

        AdministratorRequest GetAdministratorWithUserById(int id);

        AdministratorRequest GetAdministratorWithUserByEmail(EmailRequest email);

        IEnumerable<AdministratorRequest> GetAdministratorsWithUserByName(string name);
    }
}
