namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class LoanRepository : GenericRepository<MAV.Web.Data.Entities.Loan>, ILoanRepository
    {
        private readonly DataContext dataContext;

        public LoanRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IQueryable GetLoanWithAplicantAndIntern()
        {
            return this.dataContext.Loans
                .Include(t => t.Applicant.User)
                .Include(t => t.Intern.User)
                //Cambiarle lo de collection para que tome el id
                .Include(t => t.LoanDetails);
        }

        public IQueryable GetLoan()
        {
            return this.dataContext.Loans
                .Include(t => t.Id);
        }

        public MAV.Common.Models.ApplicantRequest GetLoans(EmailRequest emailApplicant)
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
                    }).Where(ld => ld.Observations != null).ToList()
                }).ToList()
            };

            return x;
        }
    }
}
