namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
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

        public IQueryable GetStatus()
        {
            return this.dataContext.Statuses;
        }

        public StatusRequest GetStatusById(int id)
        {
            var a = this.dataContext.Statuses.FirstOrDefault(s => s.Id == id);

            if (a == null)
            {
                return null;
            }

            var x = new StatusRequest
            {
                Id = a.Id,
                Name = a.Name
            };

            return x;
        }

        public IEnumerable<StatusRequest> GetStatusByName(string name)
        {
            var a = this.dataContext.Statuses
                .Where(n => n.Name == name);

            if (a == null)
            {
                return null;
            }

            var x = a.Select(a => new StatusRequest
            {
                Id = a.Id,
                Name = a.Name

            }).ToList();

            return x;
        }
    }
}
