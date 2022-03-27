using MAV.Common.Models;
using MAV.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                .Include(t => t.Loan)
                .Include(t => t.Status); 
        }

        public IQueryable GetLoanDetails()
        {
            return this.dataContext.LoanDetails
                .Where(ld => ld.DateTimeIn != null);
        }

        public async Task<LoanDetail> GetByIdAppOrInternLoanDetailsAsync(string id)
        {
            return await this.dataContext.LoanDetails
                .FirstOrDefaultAsync(e => e.Loan.Intern.User.Id == id || e.Loan.Applicant.User.Id == id);
        }

        public IEnumerable<LoanDetailsRequest> GetLoanDetailsWithMaterialWithoutDateTimeIn() //Where(ld => ld.DateTimeIn == null)
        {
            var ld = this.dataContext.LoanDetails
                .Include(ld => ld.Material)
                .ThenInclude(m => m.Status)
                .Include(ld => ld.Material)
                .ThenInclude(m => m.MaterialType)
                .Include(ld => ld.Material)
                .ThenInclude(m => m.Owner.User)
                .Where(ld => ld.DateTimeIn == null);

            if (ld == null)
            {
                return null;
            }

            var x = ld.Select(ldr => new LoanDetailsRequest
            {
                Id = ldr.Id,
                DateTimeIn = ldr.DateTimeIn,
                DateTimeOut = ldr.DateTimeOut,
                Observations = ldr.Observations,
                Material = new MaterialRequest
                {
                    Id = ldr.Material.Id,
                    Brand = ldr.Material.Brand,
                    Label = ldr.Material.Label,
                    MaterialModel = ldr.Material.MaterialModel,
                    MaterialType = ldr.Material.MaterialType.Name,
                    Name = ldr.Material.Name,
                    SerialNum = ldr.Material.SerialNum,
                    Status = ldr.Material.Status.Name,
                    Owner = ldr.Material.Owner.User.FullName
                }
            }).ToList();

            return x;
        }

        public IEnumerable<LoanDetailsRequest> GetLoansDetailsWithMaterialAndOwner()
        {
            var ld = this.dataContext.LoanDetails
                .Include(ld => ld.Material)
                .ThenInclude(m => m.Status)
                .Include(ld => ld.Material)
                .ThenInclude(m => m.MaterialType)
                .Include(ld => ld.Material)
                .ThenInclude(m => m.Owner.User);

            if (ld == null)
            {
                return null;
            }

            var x = ld.Select(ldr => new LoanDetailsRequest
            {
                Id = ldr.Id,
                DateTimeIn = ldr.DateTimeIn,
                DateTimeOut = ldr.DateTimeOut,
                Observations = ldr.Observations,
                Material = new MaterialRequest
                {
                    Id = ldr.Material.Id,
                    Brand = ldr.Material.Brand,
                    Label = ldr.Material.Label,
                    MaterialModel = ldr.Material.MaterialModel,
                    MaterialType = ldr.Material.MaterialType.Name,
                    Name = ldr.Material.Name,
                    SerialNum = ldr.Material.SerialNum,
                    Status = ldr.Material.Status.Name,
                    Owner = ldr.Material.Owner.User.FullName
                }
            }).ToList();

            return x;
        }

        public LoanDetailsRequest GetLoanDetailWithMaterialAndOwnerById(int id)
        {
            var ld = this.dataContext.LoanDetails
                .Include(ld => ld.Material)
                .ThenInclude(m => m.Status)
                .Include(ld => ld.Material)
                .ThenInclude(m => m.MaterialType)
                .Include(ld => ld.Material)
                .ThenInclude(m => m.Owner.User)
                .FirstOrDefault(ldi => ldi.Id == id);

            if (ld == null)
            {
                return null;
            }

            var x = new LoanDetailsRequest
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
            };

            return x;
        }

        public LoanDetailsRequest GetLoanDetailById(int id)
        {
            var ld = this.dataContext.LoanDetails
                .FirstOrDefault(ldi => ldi.Id == id);

            if (ld == null)
            {
                return null;
            }

            var x = new LoanDetailsRequest
            {
                Id = ld.Id,
                DateTimeIn = ld.DateTimeIn,
                DateTimeOut = ld.DateTimeOut,
                Observations = ld.Observations
            };

            return x;
        }

        public LoanDetailsRequest GetLoansDetailsWithMaterialByDateTimeOut(DateTime time)
        {
            var ld = this.dataContext.LoanDetails
                .Include(ld => ld.Material)
                .ThenInclude(m => m.Status)
                .Include(ld => ld.Material)
                .ThenInclude(m => m.MaterialType)
                .FirstOrDefault(ldt => ldt.DateTimeOut == time);

            if (ld == null)
            {
                return null;
            }

            var x = new LoanDetailsRequest
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
            };

            return x;
        }

        public LoanDetailsRequest GetLoansDetailsWithMaterialAndOwnerByNameMaterial(string nameMaterial)
        {
            var ld = this.dataContext.LoanDetails
                .Include(ld => ld.Material)
                .ThenInclude(m => m.Status)
                .Include(ld => ld.Material)
                .ThenInclude(m => m.MaterialType)
                .Include(ld => ld.Material)
                .ThenInclude(m => m.Owner.User)
                .FirstOrDefault(ldt => ldt.Material.Name == nameMaterial);

            if (ld == null)
            {
                return null;
            }

            var x = new LoanDetailsRequest
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
            };

            return x;
        }

    }
}
