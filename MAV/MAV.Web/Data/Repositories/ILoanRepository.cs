namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using System.Linq;
    using MAV.Web.Data.Entities;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface ILoanRepository : IGenericRepository<Loan>
    {
        IQueryable GetLoanWithAplicantsAndInterns();

        Task<Loan> GetByLoanIdLoanAndApplicantAsync(int id);
        Task<Loan> GetByIdAppOrInternLoansAsync(string id);
        IEnumerable<LoanRequest> GetLoans();
        //IEnumerable<LoanRequest> GetLoansWithInternsAndLoanDetails();
        //IEnumerable<LoanRequest> GetLoansWithLoanDetailsAndMaterial();
        //LoanRequest GetLoanWithLoanDetailsById(int id);
        //LoanRequest GetLoanWithLoanDetailsAndMaterialById(int id);
        //IEnumerable<LoanRequest> GetLoansWithLoanDetailsWithMaterialAndOwnerByNameMaterial(string nameMaterial);

        // LoanRequest GetLoansWithLoanDetails();

    }
}
