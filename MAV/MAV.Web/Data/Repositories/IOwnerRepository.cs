namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public interface IOwnerRepository : IGenericRepository<Owner>
    {
        IEnumerable<SelectListItem> GetComboOwners();
        MAV.Common.Models.OwnerRequest GetOwnersWithUser();
        Task<Owner> GetByIdOwnerWithMaterialsAsync(int id);
    }
}
