namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApplicantRepository : GenericRepository<Applicant>, IApplicantRepository
    {
        private readonly DataContext dataContext;

        public ApplicantRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboApplicants()
        {
            var list = this.dataContext.Applicants.Select(t => new SelectListItem
            {
                Text = t.User.FullName,
                Value = $"{t.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un solicitante...)",
                Value = "0"
            });
            return list;
        }

        public async Task<Applicant> GetByIdUserWithUserApplicantAsync(string id)
        {
            return await this.dataContext.Applicants
                .Include(t => t.User)
                .Include(l => l.Loans)
                .FirstOrDefaultAsync(e => e.User.Id == id);
        }

        public IQueryable GetApplicantsWithUser()
        {
            return this.dataContext.Applicants
                .Include(a => a.User)
                .Include(a => a.ApplicantType);
        }

        public async Task<Applicant> GetByIdWithUserAsync(int id)
        {
            return await this.dataContext.Applicants
                .Include(t => t.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        //public async Task<Applicant> GetByIdApplicantWithLoansAsync(int id)
        //{
        //    return await this.dataContext.Applicants
        //        .Include(a => a.User)
        //        .Include(l => l.Loans)
        //        .FirstOrDefaultAsync(t => t.Id == id);
        //}

        //public async Task<Applicant> GetByIdApplicantWithLoansLoanDetailsAsync(int id)
        //{
        //    return await this.dataContext.Applicants
        //        .Include(a => a.User)
        //        .Include(l => l.Loans)
        //        .ThenInclude(ld => ld.LoanDetails)
        //        .ThenInclude(m => m.Material)
        //        .FirstOrDefaultAsync(t => t.Id == id);
        //}

        public ApplicantRequest GetApplicantByEmail(EmailRequest emailApplicant)
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
                    .Include(a => a.Loans)
                    .ThenInclude(l => l.LoanDetails)
                    .ThenInclude(m => m.Material)
                    .ThenInclude(o => o.Owner.User)
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
                
            };
            //.Where(ld => ld.Observations != null)
            return x;
        }

        //public IEnumerable<ApplicantRequest> GetApplicantsWithInternLoanLoanDetailsMaterialAndOwnerByNameApplicant(string name)
        //{
        //    var a = this.dataContext.Applicants
        //            .Include(a => a.User)
        //            .Include(a => a.Loans)
        //            .ThenInclude(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.Status)
        //            .Include(a => a.Loans)
        //            .ThenInclude(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.MaterialType)
        //            .Include(a => a.Loans)
        //            .ThenInclude(l => l.Intern)
        //            .ThenInclude(a => a.User)
        //            .Include(a => a.ApplicantType)
        //            .Include(a => a.Loans)
        //            .ThenInclude(l => l.LoanDetails)
        //            .ThenInclude(m => m.Material)
        //            .ThenInclude(o => o.Owner.User)
        //            .Where(n => n.User.FirstName == name);

        //    if (a == null)
        //    {
        //        return null;
        //    }

        //    var x = a.Select(ar => new ApplicantRequest
        //    {
        //        Id = ar.Id,
        //        FirstName = ar.User.FirstName,
        //        LastName = ar.User.LastName,
        //        Email = ar.User.Email,
        //        PhoneNumber = ar.User.PhoneNumber,
        //        ApplicantType = ar.ApplicantType.Name,
        //        Loans = ar.Loans.Select(l => new LoanRequest
        //        {
        //            Id = l.Id,
        //            Intern = l.Intern.User.FullName,
        //            LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
        //            {
        //                Id = ld.Id,
        //                DateTimeIn = ld.DateTimeIn,
        //                DateTimeOut = ld.DateTimeOut,
        //                Observations = ld.Observations,
        //                Material = new MaterialRequest
        //                {
        //                    Id = ld.Material.Id,
        //                    Brand = ld.Material.Brand,
        //                    Label = ld.Material.Label,
        //                    MaterialModel = ld.Material.MaterialModel,
        //                    MaterialType = ld.Material.MaterialType.Name,
        //                    Name = ld.Material.Name,
        //                    SerialNum = ld.Material.SerialNum,
        //                    Status = ld.Material.Status.Name,
        //                    Owner = ld.Material.Owner.User.FullName
        //                }
        //            }).ToList()
        //        }).ToList()
        //    }).ToList();

        //    return x;
        //}

        //public IEnumerable<ApplicantRequest> GetApplicantsWithInternLoanLoanDetailsMaterialAndOwnerByApplicantType(string type)
        //{
        //    var a = this.dataContext.Applicants
        //            .Include(a => a.User)
        //            .Include(a => a.Loans)
        //            .ThenInclude(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.Status)
        //            .Include(a => a.Loans)
        //            .ThenInclude(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.MaterialType)
        //            .Include(a => a.Loans)
        //            .ThenInclude(l => l.Intern)
        //            .ThenInclude(a => a.User)
        //            .Include(a => a.ApplicantType)
        //            .Include(a => a.Loans)
        //            .ThenInclude(l => l.LoanDetails)
        //            .ThenInclude(m => m.Material)
        //            .ThenInclude(o => o.Owner.User)
        //            .Where(t => t.ApplicantType.Name == type);

        //    if (a == null)
        //    {
        //        return null;
        //    }

        //    var x = a.Select(a => new ApplicantRequest
        //    {
        //        Id = a.Id,
        //        FirstName = a.User.FirstName,
        //        LastName = a.User.LastName,
        //        Email = a.User.Email,
        //        PhoneNumber = a.User.PhoneNumber,
        //        ApplicantType = a.ApplicantType.Name,
        //        Loans = a.Loans.Select(l => new LoanRequest
        //        {
        //            Id = l.Id,
        //            Intern = l.Intern.User.FullName,
        //            LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
        //            {
        //                Id = ld.Id,
        //                DateTimeIn = ld.DateTimeIn,
        //                DateTimeOut = ld.DateTimeOut,
        //                Observations = ld.Observations,
        //                Material = new MaterialRequest
        //                {
        //                    Id = ld.Material.Id,
        //                    Brand = ld.Material.Brand,
        //                    Label = ld.Material.Label,
        //                    MaterialModel = ld.Material.MaterialModel,
        //                    MaterialType = ld.Material.MaterialType.Name,
        //                    Name = ld.Material.Name,
        //                    SerialNum = ld.Material.SerialNum,
        //                    Status = ld.Material.Status.Name,
        //                    Owner = ld.Material.Owner.User.FullName
        //                }
        //            }).ToList()
        //        }).ToList()
        //    }).ToList();

        //    return x;
        //}

        //public IEnumerable<ApplicantRequest> GetApplicantsWithInternLoanLoanDetailsMaterialAndOwner()
        //{
        //    var a = this.dataContext.Applicants
        //            .Include(a => a.User)
        //            .Include(a => a.Loans)
        //            .ThenInclude(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.Status)
        //            .Include(a => a.Loans)
        //            .ThenInclude(l => l.LoanDetails)
        //            .ThenInclude(ld => ld.Material)
        //            .ThenInclude(m => m.MaterialType)
        //            .Include(a => a.Loans)
        //            .ThenInclude(l => l.Intern)
        //            .ThenInclude(a => a.User)
        //            .Include(a => a.ApplicantType)
        //            .Include(a => a.Loans)
        //            .ThenInclude(l => l.LoanDetails)
        //            .ThenInclude(m => m.Material)
        //            .ThenInclude(o => o.Owner.User);

        //    if (a == null)
        //    {
        //        return null;
        //    }

        //    var x = a.Select(a => new ApplicantRequest
        //    {
        //        Id = a.Id,
        //        FirstName = a.User.FirstName,
        //        LastName = a.User.LastName,
        //        Email = a.User.Email,
        //        PhoneNumber = a.User.PhoneNumber,
        //        ApplicantType = a.ApplicantType.Name,
        //        Loans = a.Loans.Select(l => new LoanRequest
        //        {
        //            Id = l.Id,
        //            Intern = l.Intern.User.FullName,
        //            LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
        //            {
        //                Id = ld.Id,
        //                DateTimeIn = ld.DateTimeIn,
        //                DateTimeOut = ld.DateTimeOut,
        //                Observations = ld.Observations,
        //                Material = new MaterialRequest
        //                {
        //                    Id = ld.Material.Id,
        //                    Brand = ld.Material.Brand,
        //                    Label = ld.Material.Label,
        //                    MaterialModel = ld.Material.MaterialModel,
        //                    MaterialType = ld.Material.MaterialType.Name,
        //                    Name = ld.Material.Name,
        //                    SerialNum = ld.Material.SerialNum,
        //                    Status = ld.Material.Status.Name,
        //                    Owner = ld.Material.Owner.User.FullName
        //                }
        //            }).ToList()
        //        }).ToList()
        //    }).ToList();

        //    return x;
        //}

        public ApplicantRequest GetByIdApplicantWithUser(int id)
        {
            var a = this.dataContext.Applicants
                    .Include(a => a.User)
                    .Include(at => at.ApplicantType)
                    .FirstOrDefault(t => t.Id == id);

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
                ApplicantType = a.ApplicantType.Name
            };

            return x;
        }

        public IEnumerable<ApplicantRequest> GetApplicants()
        {
            var a = this.dataContext.Applicants
                    .Include(a => a.User)
                    .Include(at => at.ApplicantType);

            if (a == null)
            {
                return null;
            }
            var x = a.Select(ar => new ApplicantRequest
            {
                Id = ar.Id,
                FirstName = ar.User.FirstName,
                LastName = ar.User.LastName,
                Email = ar.User.Email,
                PhoneNumber = ar.User.PhoneNumber,
                ApplicantType = ar.ApplicantType.Name
            }).ToList();

            return x;
        }
        public Applicant GetApplicantByName(string fullname)
        {
            //var a = this.dataContext.Applicants.Include(a => a.User);
            //foreach (Applicant app in a)
            //{
            //    if (app.User.FullName == fullname)
            //    {
            //        return app;
            //    }
            //}
            var a = this.dataContext.Users;
            var b = this.dataContext.Applicants.Include(o => o.User);
            foreach (Entities.User u in a)
            {
                if (u.FullName == fullname)
                {
                    foreach (Applicant ap in b)
                    {
                        if (ap.User == u)
                        {
                            return ap;
                        }
                    }
                }
            }
            return null;
        }

    }
}
