namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;

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
    }
}
