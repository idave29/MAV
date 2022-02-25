namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public interface ILoanDetailRepository : IGenericRepository<LoanDetail>
    {
        IQueryable GetLoanDetailsWithMaterialAndLoan();
        IQueryable GetLoanDetails();
        IEnumerable<LoanDetailsRequest> GetLoanDetailsWithMaterialWithoutDateTimeIn(); //Where(ld => ld.DateTimeIn == null)
        IEnumerable<LoanDetailsRequest> GetLoansDetailsWithMaterialAndOwner();
        LoanDetailsRequest GetLoanDetailWithMaterialAndOwnerById(int id);
        LoanDetailsRequest GetLoanDetailById(int id);
        LoanDetailsRequest GetLoansDetailsWithMaterialByDateTimeOut(DateTime time);
        LoanDetailsRequest GetLoansDetailsWithMaterialAndOwnerByNameMaterial(string nameMaterial);


        //LoanDetail GetLoanDetailsWithEmail(EmailRequest emailApplicant);

    }
}
