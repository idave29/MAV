namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    public interface IStatusRepository : IGenericRepository<Status>
    {
        IEnumerable<SelectListItem> GetComboStatuses();

        MAV.Common.Models.StatusRequest GetStatus();
    }
}
