namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;

    public interface IStatusRepository : IGenericRepository<Status>
    {
        IEnumerable<SelectListItem> GetComboStatuses();
        IQueryable GetStatus();
        StatusRequest GetStatusById(int id);
        Status GetStatusByName(string name);
    }
}
