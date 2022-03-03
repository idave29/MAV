namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class InternRepository : GenericRepository<Intern>, IInternRepository
    {
        private readonly DataContext dataContext;

        public InternRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<Intern> GetByIdInternWithLoansAsync(int id)
        {
            return await this.dataContext.Interns
                .Include(t => t.User)
                .Include(t => t.Loans)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Intern> GetByIdInternWithLoansLoansDetailsAsync(int id)
        {
            return await this.dataContext.Interns
                .Include(t => t.User)
                .Include(t => t.Loans)
                .ThenInclude(c => c.LoanDetails)
                .ThenInclude(cd => cd.Material)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<Intern> GetByIdWithUserAsync(int id)
        {
            return this.dataContext.Interns
                 .Include(t => t.User)
                 .FirstOrDefaultAsync(e => e.Id == id);
        }

        public IEnumerable<SelectListItem> GetComboInterns()
        {
            var list = this.dataContext.Interns.Select(t => new SelectListItem
            {
                Text = t.User.FullName,
                Value = $"{t.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un becario...)",
                Value = "0"
            });
            return list;
        }

        public IQueryable GetInternsWithUser()
        {
            return this.dataContext.Interns
                .Include(i => i.User);
        }

        public IEnumerable<InternRequest> GetInterns()
        {
            var i = this.dataContext.Interns
                .Include(i => i.User);

            if (i == null)
            {
                return null;
            }

            var x = i.Select(a => new InternRequest
            {
                Id = a.Id,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                Email = a.User.Email,
                PhoneNumber = a.User.PhoneNumber
            }).ToList();

            return x;

        }


        public IEnumerable<InternRequest> GetInternsWithLoansLoanDetailsWithMaterialAndOwner()
        {
            var i = this.dataContext.Interns
                .Include(i => i.User)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails);

            if (i == null)
            {
                return null;
            }

            var x = i.Select(a => new InternRequest
            {
                Id = a.Id,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                Email = a.User.Email,
                PhoneNumber = a.User.PhoneNumber,
                Loans = a.Loans.Select(l => new LoanRequest
                {
                    Id = l.Id,
                    LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
                    {
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
                            Status = ld.Material.Status.Name,
                            Owner = ld.Material.Owner.User.FullName
                        }
                    }).ToList()
                }).ToList()
            }).ToList();

            return x;
        }

        public InternRequest GetInternWithLoansLoanDetailsWithMaterialAndOwnerByEmail(EmailRequest emailIntern)
        {
            var i = this.dataContext.Interns
                .Include(i => i.User)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.Status)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.MaterialType)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.Owner.User)
                .FirstOrDefault(i => i.User.Email.ToLower() == emailIntern.Email);

            if (i == null)
            {
                return null;
            }
            var x = new InternRequest
            {
                Id = i.Id,
                FirstName = i.User.FirstName,
                LastName = i.User.LastName,
                Email = i.User.Email,
                PhoneNumber = i.User.PhoneNumber,
                Loans = i.Loans?.Select(l => new LoanRequest
                {
                    Id = l.Id,
                    LoanDetails = l.LoanDetails?.Select(ld => new LoanDetailsRequest
                    {
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
                            Status = ld.Material.Status.Name,
                            Owner = ld.Material.Owner.User.FullName
                        }
                    }).ToList()
                }).ToList()
            };

            return x;
        }

        public InternRequest GetInternWithLoansByEmail(EmailRequest emailIntern)
        {
            var i = this.dataContext.Interns
                .Include(i => i.User)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.Status)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.MaterialType)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.Owner.User)
                .Include(i => i.Loans)
                .ThenInclude(a => a.Applicant.User)
                .FirstOrDefault(i => i.User.Email.ToLower() == emailIntern.Email);

            if (i == null)
            {
                return null;
            }
            var x = new InternRequest
            {
                Id = i.Id,
                FirstName = i.User.FirstName,
                LastName = i.User.LastName,
                Email = i.User.Email,
                PhoneNumber = i.User.PhoneNumber,
                Loans = i.Loans?.Select(l => new LoanRequest
                {
                    Id = l.Id,
                    Intern = l.Intern.User.FullName,
                    Applicant = l.Applicant.User.FullName,
                    LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
                    {
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
                            Status = ld.Material.Status.Name,
                            Owner = ld.Material.Owner.User.FullName
                        }
                    }).ToList()
                }).ToList()
            };

            return x;
        }

        public IEnumerable<InternRequest> GetInternsWithLoansLoanDetailsWithMaterialAndOwnerByName(string name)
        {
            var i = this.dataContext.Interns
                .Include(i => i.User)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.Status)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.MaterialType)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.Owner.User)
                .Where(u => u.User.FirstName == name);

            if (i == null)
            {
                return null;
            }

            var x = i.Select( a => new InternRequest
            {
                Id = a.Id,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                Email = a.User.Email,
                PhoneNumber = a.User.PhoneNumber,
                Loans = a.Loans.Select(l => new LoanRequest
                {
                    Id = l.Id,
                    LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
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
                            Status = ld.Material.Status.Name,
                            Owner = ld.Material.Owner.User.FullName
                        }
                    }).ToList()
                }).ToList()
            }).ToList();

            return x;
        }

        public IEnumerable<InternRequest> GetInternsWithLoansLoanDetailsWithMaterialAndOwnerById(int id)
        {
            var i = this.dataContext.Interns
                .Include(i => i.User)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.Status)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.MaterialType)
                .Include(i => i.Loans)
                .ThenInclude(l => l.LoanDetails)
                .ThenInclude(ld => ld.Material)
                .ThenInclude(m => m.Owner.User)
                .Where(u => u.Id == id);

            if (i == null)
            {
                return null;
            }

            var x = i.Select(a => new InternRequest
            {
                Id = a.Id,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                Email = a.User.Email,
                PhoneNumber = a.User.PhoneNumber,
                Loans = a.Loans.Select(l => new LoanRequest
                {
                    Id = l.Id,
                    Intern = l.Intern.User.FullName,
                    LoanDetails = l.LoanDetails.Select(ld => new LoanDetailsRequest
                    {
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
                            Status = ld.Material.Status.Name,
                            Owner = ld.Material.Owner.User.FullName
                        }
                    }).ToList()
                }).ToList()
            }).ToList();

            return x;
        }
    }
}
