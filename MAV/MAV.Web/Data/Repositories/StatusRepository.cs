using MAV.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAV.Web.Data.Repositories
{
    public class StatusRepository : GenericRepository<Status>, IStatusRepository
    {
        private readonly DataContext dataContext;

        public StatusRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IQueryable GetAllWithCourses()
        {
            return this.dataContext.Subjects
                .Include(s => s.Courses);
        }

        public IEnumerable<SelectListItem> GetComboSubjects()
        {
            var list = this.dataContext.Subjects.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = $"{s.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona una materia...)",
                Value = "0"
            });
            return list;
        }
    }
}
