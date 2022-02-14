namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using System.Linq;
    public interface ILoanDetailRepository : IGenericRepository<LoanDetail>
    {
        //IEnumerable<SelectListItem> GetComboMaterial();

        IQueryable GetLoanDetailsWithMaterialAndLoan();

        IQueryable GetLoanDetails();

        MAV.Common.Models.ApplicantRequest GetLoanDetailsWithEmail(EmailRequest emailApplicant);
    }
}
