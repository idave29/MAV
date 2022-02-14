using MAV.Common.Models;
using MAV.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MAV.Web.Data.Repositories
{
    public class LoanDetailRepository : GenericRepository<LoanDetail>, ILoanDetailRepository
    {
        private readonly DataContext dataContext;

        public LoanDetailRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IQueryable GetLoanDetailsWithMaterialAndLoan()
        {
            return this.dataContext.LoanDetails
                .Include(t => t.Material)
                .Include(t => t.Loan);
        }

        public IQueryable GetLoanDetails()
        {
            return this.dataContext.LoanDetails
                .Include(ld => ld.Loan)
                .ThenInclude(l => l.Applicant)
                .ThenInclude(a => a.User)
                .Include(ld => ld.Material)
                .Where(ld => ld.DateTimeIn != null);
        }

        public MAV.Common.Models.ApplicantRequest GetLoanDetailsWithEmail(EmailRequest emailApplicant)
        {
            var a = this.dataContext.Applicants
                    .Include(a => a.User)
                    .Include(a => a.Loans)
                    .ThenInclude(l => l.LoanDetails)
                    .ThenInclude(ld => ld.Material)
                    .ThenInclude(m => m.Status)
                    .Include(a => a.Loans)
                    .ThenInclude(l => l.LoanDetails)
                    .ThenInclude(ld => ld.Material)
                    .ThenInclude(m => m.MaterialType)
                    .Include(a => a.Loans)
                    .ThenInclude(l => l.Intern)
                    .ThenInclude(a => a.User)
                    .Include(a => a.ApplicantType)
                    .FirstOrDefault(a => a.User.Email.ToLower() == emailApplicant.Email);

            if (a == null)
            {
                return null;
            }
            var x = new ApplicantRequest
            {
                Id = a.Id,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                Email = a.User.Email,
                PhoneNumber = a.User.PhoneNumber,
                ApplicantType = a.ApplicantType.Name,
                Loans = a.Loans?.Select(l => new LoanRequest
                {
                    Id = l.Id,
                    Intern = new InternRequest
                    {
                        Id = l.Intern.Id,
                        Email = l.Intern.User.Email,
                        FirstName = l.Intern.User.FirstName,
                        LastName = l.Intern.User.LastName,
                        PhoneNumber = l.Intern.User.PhoneNumber,
                    },
                    LoanDetails = l.LoanDetails?.Select(ld => new LoanDetailsRequest
                    {
                        Id = ld.Id,
                        DateTimeIn = ld.DateTimeIn,
                        DateTimeOut = ld.DateTimeOut,
                        Observations = ld.Observations,
                        Material = new MaterialRequest
                        {
                            Id = ld.Material.Id,
                            Brand = ld.Material.Brand,
                            Label = ld.Material.Label,
                            MaterialModel = ld.Material.MaterialModel,
                            MaterialType = ld.Material.MaterialType.Name,
                            Name = ld.Material.Name,
                            SerialNum = ld.Material.SerialNum,
                            Status = ld.Material.Status.Name
                        }
                    }).Where(ld => ld.Id == a.Id).ToList()
                }).ToList()
            };

            return x;
        }
    }
}
