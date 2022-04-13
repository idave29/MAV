namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MaterialRepository : GenericRepository<Material>, IMaterialRepository
    {
        private readonly DataContext dataContext;

        public MaterialRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboMaterial()
        {
            var list = this.dataContext.Materials.Select(t => new SelectListItem
            {
                Text = t.Label,
                Value = $"{t.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un material...)",
                Value = "0"
            });
            return list;
        }

        //public async Task<Material> GetByIdMaterialsAsync(int id)
        //{
        //    return await this.dataContext.Materials
        //        .FirstOrDefault(m => m.Id == id);
        //}

        public IQueryable GetMaterialsWithTypeWithStatusAndOwner()
        {
            return this.dataContext.Materials
                .Include(t => t.MaterialType)
                .Include(t => t.Status)
                .Include(t => t.Owner.User);
        }

        public IQueryable GetMaterials()
        {
            return this.dataContext.Materials;
        }

        public IQueryable GetMaterialsWithOwner()
        {
            return this.dataContext.Materials
                .Include(t => t.Owner.User);
        }

        public async Task<Material> GetByIdWithMaterialTypeOwnerStatusAsync(int id)
        {
            return await this.dataContext.Materials
                .Include(t => t.MaterialType)
                .Include(t => t.Status)
                .Include(t => t.Owner.User)
                .Include(t => t.LoanDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
        }


        public MaterialRequest GetMaterialWithTypeWithStatusAndOwnerById(int id)
        {
            var m = this.dataContext.Materials
                    .Include(s => s.Status)
                    .Include(mt => mt.MaterialType)
                    .Include(o => o.Owner.User)
                    .FirstOrDefault(i => i.Id == id);

            if (m == null)
            {
                return null;
            }

            var x = new MaterialRequest
            {
                Id = m.Id,
                Name = m.Name,
                Label = m.Label,
                Brand = m.Brand,
                MaterialModel = m.MaterialModel,
                SerialNum = m.SerialNum,
                Status = m.Status.Name,
                MaterialType = m.MaterialType.Name,
                Owner = m.Owner.User.FullName
            };

            return x;
        }

        public MaterialRequest GetMaterialWithTypeAndStatusBySerialNum(string num)
        {
            var m = this.dataContext.Materials
                    .Include(s => s.Status)
                    .Include(m => m.MaterialType)
                    .FirstOrDefault(n => n.SerialNum == num);

            if (m == null)
            {
                return null;
            }

            var x = new MaterialRequest
            {
                Id = m.Id,
                Name = m.Name,
                Label = m.Label,
                Brand = m.Brand,
                MaterialModel = m.MaterialModel,
                SerialNum = m.SerialNum,
                Status = m.Status.Name,
                MaterialType = m.MaterialType.Name
            };

            return x;
        }

        public MaterialRequest GetMaterialBySerialNum(string num)
        {
            var m = this.dataContext.Materials
                    .FirstOrDefault(n => n.SerialNum == num);

            if (m == null)
            {
                return null;
            }

            var x = new MaterialRequest
            {
                Id = m.Id,
                Name = m.Name,
                Label = m.Label,
                Brand = m.Brand,
                MaterialModel = m.MaterialModel,
                SerialNum = m.SerialNum
            };
            return x;
        }

        public IEnumerable<MaterialRequest> GetMaterialsWithTypeWithStatusAndOwnerByBrand(string brand)
        {
            var m = this.dataContext.Materials
                .Include(s => s.Status)
                .Include(m => m.MaterialType)
                .Include(o => o.Owner)
                .Where(b => b.Brand == brand); 

            if (m == null)
            {
                return null;
            }

            var x = m.Select(mr => new MaterialRequest
            {
                Id = mr.Id,
                Name = mr.Name,
                Label = mr.Label,
                Brand = mr.Brand,
                MaterialModel = mr.MaterialModel,
                SerialNum = mr.SerialNum,
                Status = mr.Status.Name,
                MaterialType = mr.MaterialType.Name,
                Owner = mr.Owner.User.FullName
            }).ToList();

            return x;
        }

        public IEnumerable<MaterialRequest> GetMaterialsWithTypeWithStatusAndOwnerByStatus(string status)
        {
            var m = this.dataContext.Materials
                .Include(s => s.Status)
                .Include(m => m.MaterialType)
                .Include(o => o.Owner)
                .Where(s => s.Status.Name == status);

            if (m == null)
            {
                return null;
            }

            var x = m.Select(mr => new MaterialRequest
            {
                Id = mr.Id,
                Name = mr.Name,
                Label = mr.Label,
                Brand = mr.Brand,
                MaterialModel = mr.MaterialModel,
                SerialNum = mr.SerialNum,
                Status = mr.Status.Name,
                MaterialType = mr.MaterialType.Name,
                Owner = mr.Owner.User.FullName
            }).ToList();

            return x;
        }

        public IEnumerable<MaterialRequest> GetMaterialsWithTypeWithStatusAndOwnerByType(string type)
        {
            var m = this.dataContext.Materials
                .Include(s => s.Status)
                .Include(m => m.MaterialType)
                .Include(o => o.Owner)
                .Where(mt => mt.MaterialType.Name == type);

            if (m == null)
            {
                return null;
            }

            var x = m.Select(mr => new MaterialRequest
            {
                Id = mr.Id,
                Name = mr.Name,
                Label = mr.Label,
                Brand = mr.Brand,
                MaterialModel = mr.MaterialModel,
                SerialNum = mr.SerialNum,
                Status = mr.Status.Name,
                MaterialType = mr.MaterialType.Name,
                Owner = mr.Owner.User.FullName
            }).ToList();

            return x;
        }

        public IEnumerable<MaterialRequest> GetMaterialsWithTypeWithStatusAndOwnerByName(string name)
        {
            var m = this.dataContext.Materials
                .Include(s => s.Status)
                .Include(m => m.MaterialType)
                .Include(o => o.Owner)
                .Where(n => n.Name == name);

            if (m == null)
            {
                return null;
            }

            var x = m.Select(mr => new MaterialRequest
            {
                Id = mr.Id,
                Name = mr.Name,
                Label = mr.Label,
                Brand = mr.Brand,
                MaterialModel = mr.MaterialModel,
                SerialNum = mr.SerialNum,
                Status = mr.Status.Name,
                MaterialType = mr.MaterialType.Name,
                Owner = mr.Owner.User.FullName
            }).ToList();

            return x;
        }

        public IEnumerable<MaterialRequest> GetMaterialsWithTypeWithStatusAndOwnerByLabel(string label)
        {
            var m = this.dataContext.Materials
                .Include(s => s.Status)
                .Include(m => m.MaterialType)
                .Include(o => o.Owner)
                .Where(n => n.Label == label);

            if (m == null)
            {
                return null;
            }

            var x = m.Select(mr => new MaterialRequest
            {
                Id = mr.Id,
                Name = mr.Name,
                Label = mr.Label,
                Brand = mr.Brand,
                MaterialModel = mr.MaterialModel,
                SerialNum = mr.SerialNum,
                Status = mr.Status.Name,
                MaterialType = mr.MaterialType.Name,
                Owner = mr.Owner.User.FullName
            }).ToList();

            return x;
        }

        public IEnumerable<MaterialRequest> GetAllMaterialsWithTypeWithStatusAndOwner()
        {
            var m = this.dataContext.Materials
                .Include(s => s.Status)
                .Include(m => m.MaterialType)
                .Include(o => o.Owner.User);

            if (m == null)
            {
                return null;
            }

            var x = m.Select(mr => new MaterialRequest
            {
                Id = mr.Id,
                Name = mr.Name,
                Label = mr.Label,
                Brand = mr.Brand,
                Function = mr.Function,
                ImageURL = mr.ImageURL,
                MaterialModel = mr.MaterialModel,
                SerialNum = mr.SerialNum,
                Status = mr.Status.Name,
                MaterialType = mr.MaterialType.Name,
                Owner = mr.Owner.User.FullName
            }).ToList();

            return x;
        }

        public IEnumerable<LoanRequest> GetMaterialWithLoansById(int id)
        {
            var m = this.dataContext.Loans
                    .Include(a => a.Applicant.User)
                    .Include(a => a.Intern.User)
                    .Include(l => l.LoanDetails)
                    .ThenInclude(ld => ld.Material)
                    .ThenInclude(m => m.Status)
                    .Include(l => l.LoanDetails)
                    .ThenInclude(ld => ld.Material)
                    .ThenInclude(m => m.MaterialType)
                    .Include(l => l.LoanDetails)
                    .ThenInclude(ld => ld.Material)
                    .ThenInclude(m => m.Owner.User);

            //var m1 = this.dataContext.Materials
            //    .Include(s => s.Status)
            //    .Include(m => m.MaterialType)
            //    .Include(o => o.Owner.User)
            //    .Include(ld => ld.LoanDetails)
            //    .ThenInclude(l => l.Loan)
            //    .ThenInclude(i => i.Intern.User)
            //    .Include(ld => ld.LoanDetails)
            //    .ThenInclude(l => l.Loan)
            //    .ThenInclude(i => i.Applicant.User);

            if (m == null)
            {
                return null;
            }

            //var x1 = m1.Select(m => new MaterialRequest
            //{
            //    Id = m.Id,
            //    Brand = m.Brand,
            //    Label = m.Label,
            //    MaterialModel = m.MaterialModel,
            //    MaterialType = m.MaterialType.Name,
            //    Name = m.Name,
            //    SerialNum = m.SerialNum,
            //    Status = m.Status.Name,
            //    Owner = m.Owner.User.FullName
            //});


            var x = m.Select(m => new LoanRequest
            {
                Id = m.Id,
                Intern = m.Intern.User.FullName,
                Applicant = m.Applicant.User.FullName,
                
            }).ToList();
        
            return x;
        }

        public IEnumerable<LoanRequest> GetMaterialWithLoans()
        {
            var ml = this.dataContext.Loans
                    .Include(a => a.Applicant.User)
                    .Include(a => a.Intern.User)
                    .Include(l => l.LoanDetails)
                    .ThenInclude(ld => ld.Material)
                    .ThenInclude(m => m.Status)
                    .Include(l => l.LoanDetails)
                    .ThenInclude(ld => ld.Material)
                    .ThenInclude(m => m.MaterialType)
                    .Include(l => l.LoanDetails)
                    .ThenInclude(ld => ld.Material)
                    .ThenInclude(m => m.Owner.User);

            if (ml == null)
            {
                return null;
            }

            var x = ml.Select(m => new LoanRequest
            {
                Id = m.Id,
                Intern = m.Intern.User.FullName,
                Applicant = m.Applicant.User.FullName,
                
            }).ToList();

            return x;
        }
    }
}
