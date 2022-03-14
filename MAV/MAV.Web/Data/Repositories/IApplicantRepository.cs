namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public interface IApplicantRepository : IGenericRepository<Applicant>
    {
        IEnumerable<SelectListItem> GetComboApplicants();
        //Task<Applicant> GetByIdApplicantWithLoansAsync(int id);
        //Task<Applicant> GetByIdApplicantWithLoansLoanDetailsAsync(int id);
        IQueryable GetApplicantsWithUser();
        ApplicantRequest GetApplicantByEmail(EmailRequest emailApplicant);
        //IEnumerable<ApplicantRequest> GetApplicantsWithInternLoanLoanDetailsMaterialAndOwnerByNameApplicant(string name);
        //IEnumerable<ApplicantRequest> GetApplicantsWithInternLoanLoanDetailsMaterialAndOwnerByApplicantType(string type);
        //IEnumerable<ApplicantRequest> GetApplicantsWithInternLoanLoanDetailsMaterialAndOwner();
        ApplicantRequest GetByIdApplicantWithUser(int id);
        IEnumerable<ApplicantRequest> GetApplicants();
        Applicant GetApplicantByName(string fullname);
    }
}
