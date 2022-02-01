namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using System.Linq;

    public interface ILoanRepository : IGenericRepository<MAV.Web.Data.Entities.Loan>
    {
        IQueryable GetLoanWithAplicantAndIntern();

        IQueryable GetLoan();

        IQueryable GetApplicantLoansByEmail(EmailRequest emailRequest);
    }
}
