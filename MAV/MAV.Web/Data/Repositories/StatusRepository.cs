namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;

    public class StatusRepository : GenericRepository<Status>, IStatusRepository
    {
        private readonly DataContext dataContext;

        public StatusRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboStatuses()
        {
            var list = this.dataContext.Statuses.Select(st => new SelectListItem
            {
                Text = st.Name,
                Value = $"{st.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Status...)",
                Value = "0"
            });
            return list;
        }
    }
}
