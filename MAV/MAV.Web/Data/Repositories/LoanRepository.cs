namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LoanRepository : GenericRepository<MAV.Web.Data.Entities.Loan>, ILoanRepository
    {
        private readonly DataContext dataContext;

        public LoanRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IQueryable GetLoanWithAplicantsAndInterns()
        {
            //return this.dataContext.Loans
            //    .Include(t => t.Applicant.User)
            //    .Include(t => t.Intern.User);
            //Cambiarle lo de collection para que tome el id
            //.Include(t => t.LoanDetails);

            //return dataContext.Loans
            //     .Include(s => s.Applicant)
            //     .ThenInclude(c => c.User)
            //     .Include(s => s.Intern).ThenInclude(c => c.User)
            //     .Include(s => s.LoanDetails).ThenInclude(c => c.Material)
            //     .Include(s => s.LoanDetails).ThenInclude(c => c.Status);
            return dataContext.Loans
                 .Include(s => s.Applicant.User)
                 .Include(s => s.Intern.User)
                 .Include(s => s.LoanDetails).ThenInclude(c => c.Material)
                 .Include(s => s.LoanDetails).ThenInclude(c => c.Status);
        }

        //public async Loan GetLoanWithAplicantsAndInternsAsync(int id)
        //{
        //    return dataContext.Loans
        //         .Include(s => s.Applicant)
        //         .ThenInclude(c => c.User)
        //         .Include(s => s.Intern).ThenInclude(c => c.User)
        //         .Include(s => s.LoanDetails).ThenInclude(c => c.Material)
        //         .Include(s => s.LoanDetails).ThenInclude(c => c.Status);
        //}

        public async Task<Loan> GetByLoanIdLoanAndApplicantAsync(int id)
        {
            return await dataContext.Loans
                .Include(s => s.Applicant)
                .ThenInclude(c => c.User)
                .Include(s => s.Intern).ThenInclude(c => c.User)
                .Include(c => c.LoanDetails)
                .ThenInclude(v => v.Status)
                .Include(c => c.LoanDetails)
                .ThenInclude(v => v.Material)
                .Include(c => c.LoanDetails)
                .ThenInclude(v => v.Loan)
                .ThenInclude(x => x.Applicant)
                .ThenInclude(y => y.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Loan> GetByIdAppOrInternLoansAsync(string id)
        {
            return await this.dataContext.Loans
                .Include(t => t.LoanDetails)
                .FirstOrDefaultAsync(e => e.Intern.User.Id == id || e.Applicant.User.Id == id);
        }

        public IEnumerable<LoanRequest> GetLoans()
        {
            var l = this.dataContext.Loans
                    .Include(i => i.Intern.User)
                    .Include(a => a.Applicant.User)
                    .Include(l => l.LoanDetails)
                    .ThenInclude(ld => ld.Material)
                    .ThenInclude(m => m.Status)
                    .Include(l => l.LoanDetails)
                    .ThenInclude(ld => ld.Material)
                    .ThenInclude(m => m.MaterialType)
                    .Include(l => l.LoanDetails)
                    .ThenInclude(ld => ld.Material)
                    .ThenInclude(m => m.Owner.User);

            if (l == null)
            {
                return null;
            }

            var x = l.Select(l => new LoanRequest
            {
                Id = l.Id,
                Intern = l.Intern.User.FullName,
                Applicant = l.Applicant.User.FullName,
                LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
                {
                    Id = ld.Id,
                    DateTimeIn = ld.DateTimeIn,
                    DateTimeOut = ld.DateTimeOut,
                    Observations = ld.Observations,
                    Status = ld.Status.Name,
                    Material = new MaterialResponse
                    {
                        Id = ld.Material.Id,
                        Brand = ld.Material.Brand,
                        Function = ld.Material.Function,
                        ImageURL = ld.Material.ImageURL,
                        Label = ld.Material.Label,
                        MaterialModel = ld.Material.MaterialModel,
                        //MaterialType = ld.Material.MaterialType.Name,
                        Name = ld.Material.Name,
                        SerialNum = ld.Material.SerialNum,
                        MaterialType = ld.Material.MaterialType.Name,
                        Status = ld.Material.Status.Name,
                        Owner = ld.Material.Owner.User.Email
                    }
                }).ToList()

            }).ToList();

            return x;
        }

        //public IEnumerable<LoanRequest> GetLoansWithInternsAndLoanDetails()
        //{
        //    var l = this.dataContext.Loans
        //            .Include(l => l.LoanDetails)
        //            .Include(l => l.Intern.User);

        //    if (l == null)
        //    {
        //        return null;
        //    }

        //    var x = l.Select(l => new LoanRequest
        //    {
        //        Id = l.Id,
        //        Intern = l.Intern.User.FullName,
        //        LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
        //        {
        //            DateTimeIn = ld.DateTimeIn,
        //            DateTimeOut = ld.DateTimeOut,
        //            Observations = ld.Observations
        //        }).ToList()
        //    }).ToList();

        //    return x;
        //}


        //public IEnumerable<LoanRequest> GetLoansWithLoanDetailsAndMaterial()
        //{
        //    var l = this.dataContext.Loans
        //            .Include(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.Status)
        //            .Include(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.MaterialType)
        //            .Include(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.Owner.User);

        //    if (l == null)
        //    {
        //        return null;
        //    }

        //    var x = l.Select(l => new LoanRequest
        //    {
        //        Id = l.Id,
        //        LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
        //        {
        //            DateTimeIn = ld.DateTimeIn,
        //            DateTimeOut = ld.DateTimeOut,
        //            Observations = ld.Observations,
        //            Material = new MaterialRequest
        //            {
        //                Id = ld.Material.Id,
        //                Brand = ld.Material.Brand,
        //                Label = ld.Material.Label,
        //                MaterialModel = ld.Material.MaterialModel,
        //                MaterialType = ld.Material.MaterialType.Name,
        //                Name = ld.Material.Name,
        //                SerialNum = ld.Material.SerialNum,
        //                Status = ld.Material.Status.Name,
        //                Owner = ld.Material.Owner.User.FullName
        //            }
        //        }).ToList()
        //    }).ToList();

        //    return x;
        //}

        //public LoanRequest GetLoanWithLoanDetailsById(int id)
        //{
        //    var l = this.dataContext.Loans
        //            .Include(l => l.LoanDetails)
        //            .FirstOrDefault(t => t.Id == id);

        //    if (l == null)
        //    {
        //        return null;
        //    }

        //    var x = new LoanRequest
        //    {
        //        Id = l.Id,
        //        LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
        //        {
        //            DateTimeIn = ld.DateTimeIn,
        //            DateTimeOut = ld.DateTimeOut,
        //            Observations = ld.Observations
        //        }).ToList()
        //    };

        //    return x;
        //}

        //public LoanRequest GetLoanWithLoanDetailsAndMaterialById(int id)
        //{
        //    var l = this.dataContext.Loans
        //            .Include(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.Status)
        //            .Include(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.MaterialType)
        //            .Include(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.Owner.User)
        //            .FirstOrDefault(t => t.Id == id);

        //    if (l == null)
        //    {
        //        return null;
        //    }

        //    var x = new LoanRequest
        //    {
        //        Id = l.Id,
        //        LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
        //        {
        //            DateTimeIn = ld.DateTimeIn,
        //            DateTimeOut = ld.DateTimeOut,
        //            Observations = ld.Observations,
        //            Material = new MaterialRequest
        //            {
        //                Id = ld.Material.Id,
        //                Brand = ld.Material.Brand,
        //                Label = ld.Material.Label,
        //                MaterialModel = ld.Material.MaterialModel,
        //                MaterialType = ld.Material.MaterialType.Name,
        //                Name = ld.Material.Name,
        //                SerialNum = ld.Material.SerialNum,
        //                Status = ld.Material.Status.Name,
        //                Owner = ld.Material.Owner.User.FullName
        //            }
        //        }).ToList()
        //    };

        //    return x;
        //}

        //public IEnumerable<LoanRequest> GetLoansWithLoanDetailsWithMaterialAndOwnerByNameMaterial(string nameMaterial)
        //{
        //    var l = this.dataContext.Loans
        //            .Include(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.Status)
        //            .Include(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.MaterialType);

        //    if (l == null)
        //    {
        //        return null;
        //    }

        //    var x = l.Select(l => new LoanRequest
        //    {
        //        Id = l.Id,
        //        LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
        //        {
        //            DateTimeIn = ld.DateTimeIn,
        //            DateTimeOut = ld.DateTimeOut,
        //            Observations = ld.Observations,
        //            Material = new MaterialRequest
        //            {
        //                Id = ld.Material.Id,
        //                Brand = ld.Material.Brand,
        //                Label = ld.Material.Label,
        //                MaterialModel = ld.Material.MaterialModel,
        //                MaterialType = ld.Material.MaterialType.Name,
        //                Name = ld.Material.Name,
        //                SerialNum = ld.Material.SerialNum,
        //                Status = ld.Material.Status.Name,
        //                Owner = ld.Material.Owner.User.FullName
        //            }
        //        }).Where(m => m.Material.Name == nameMaterial).ToList()
        //    }).ToList();

        //    return x;
        //}

    }
}
