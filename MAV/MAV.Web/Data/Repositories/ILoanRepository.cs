namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ILoanRepository : IGenericRepository<MAV.Web.Data.Entities.Loan>
    {
        IQueryable GetLoanWithAplicantAndIntern();

        IQueryable GetLoan();

        Task<MAV.Web.Data.Entities.Applicant> GetApplicantLoansByEmail(EmailRequest emailRequest);

        MAV.Common.Models.ApplicantRequest GetLoans (EmailRequest emailApplicant);
    }
}
