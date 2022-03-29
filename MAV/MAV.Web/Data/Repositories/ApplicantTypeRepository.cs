namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApplicantTypeRepository : GenericRepository<ApplicantType>, IApplicantTypeRepository
    {
        private readonly DataContext dataContext;

        public ApplicantTypeRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboApplicantTypes()
        {
            var list = this.dataContext.ApplicantTypes.Select(at => new SelectListItem
            {
                Text = at.Name,
                Value = $"{at.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Select an Applicant Type...)",
                Value = "0"
            });
            return list;
        }

        public IQueryable GetAplicantTypes()
        {
            return this.dataContext.ApplicantTypes;
        }

        public async Task<ApplicantType> GetByIdAplicantTypeAsync(int id)
        {
            return await this.dataContext.ApplicantTypes
                .Include(at => at.Applicants)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public ApplicantTypeRequest GetApplicantTypeById(int id)
        {
            var a = this.dataContext.ApplicantTypes.FirstOrDefault(ap => ap.Id == id);

            if (a == null)
            {
                return null;
            }

            var x = new ApplicantTypeRequest
            {
                Id = a.Id,
                Name = a.Name
            };

            return x;
        }

        public IEnumerable<ApplicantTypeRequest> GetApplicantTypesByName(string name)
        {
            var a = this.dataContext.ApplicantTypes
                .Where(n => n.Name == name);

            if (a == null)
            {
                return null;
            }

            var x = this.dataContext.ApplicantTypes.Select(a => new ApplicantTypeRequest
            {
                Id = a.Id,
                Name = a.Name

            }).ToList();

            return x;
        }

        public ApplicantType GetApplicantTypeByName(string name)
        {
            var a = this.dataContext.ApplicantTypes;
            foreach (ApplicantType s in a)
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
