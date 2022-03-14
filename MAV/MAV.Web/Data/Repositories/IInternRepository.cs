namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public interface IInternRepository : IGenericRepository<Intern>
    {
        IEnumerable<SelectListItem> GetComboInterns();
        IEnumerable<InternRequest> GetInterns();
        IQueryable GetInternsWithUser();
        Task<Intern> GetByIdWithUserAsync(int id);

        InternRequest GetInternWithUserById(int id);

        InternRequest GetInternWithUserByEmail(EmailRequest email);

        IEnumerable<InternRequest> GetInternsWithUserByName(string name);
        //-----------

        IEnumerable<InternRequest> GetInternsWithLoansLoanDetailsWithMaterialAndOwner();
        InternRequest GetInternWithLoansLoanDetailsWithMaterialAndOwnerByEmail(EmailRequest emailIntern);
        InternRequest GetInternWithLoansByEmail(EmailRequest emailIntern);
        IEnumerable<InternRequest> GetInternsWithLoansLoanDetailsWithMaterialAndOwnerByName(string name);
        IEnumerable<InternRequest> GetInternsWithLoansLoanDetailsWithMaterialAndOwnerById(int id);
        //Task<Intern> GetByIdInternWithUserAsync(int id);
        Task<Intern> GetByIdInternWithLoansLoansDetailsAsync(int id);

    }
}
