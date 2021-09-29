namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    interface IApplicantRepository : IGenericRepository<Applicant>
    {
        IEnumerable<SelectListItem> GetComboApplicants();
        IQueryable GetApplicantsWithUser();
        Task<Applicant> GetByIdApplicantWithLoansAsync(int id);
        Task<Applicant> GetByIdApplicantWithLoansLoanDetailsAsync(int id);
    }
}
