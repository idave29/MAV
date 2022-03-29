namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        public async Task<Status> GetByIdStatusAsync(int id)
        {
            return await this.dataContext.Statuses
                .Include(st => st.Materials)
                .Include(st => st.LoanDetails)
                .FirstOrDefaultAsync(e => e.Id == id);
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

        //public IEnumerable<StatusRequest> GetStatusByName(string name)
        //{
        //    var a = this.dataContext.Statuses
        //        .Where(n => n.Name == name);

        //    if (a == null)
        //    {
        //        return null;
        //    }

        //    var x = a.Select(a => new StatusRequest
        //    {
        //        Id = a.Id,
        //        Name = a.Name

        //    }).ToList();

        //    return x;
        //}
        public Status GetStatusByName(string name)
        {
            var a = this.dataContext.Statuses;
            //var si = new Status();
            foreach (Status s in a)
            {
                if (s.Name == name)
                {
                    return s;
                }
            }
            return null;
        }
    }
}
