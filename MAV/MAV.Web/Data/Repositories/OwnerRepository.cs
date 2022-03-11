namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class OwnerRepository : GenericRepository<MAV.Web.Data.Entities.Owner>, IOwnerRepository
    {
        private readonly DataContext dataContext;

        public OwnerRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<Owner> GetByIdOwnerWithMaterialsAsync(int id)
        {
            return await this.dataContext.Owners
                .Include(t => t.User)
                //.Include(t => t.Materials)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public IEnumerable<SelectListItem> GetComboOwners()
        {
            var list = this.dataContext.Owners.Select(t => new SelectListItem
            {
                Text = t.User.FullName,
                Value = $"{t.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un propietario...)",
                Value = "0"
            });
            return list;
        }

        public IQueryable GetOwnersWithUser()
        {
            return this.dataContext.Owners
                .Include(a => a.User);
        }

        public IEnumerable<OwnerRequest> GetOwners()
        {
            var o = this.dataContext.Owners
                .Include(a => a.User);

            if (o == null)
            {
                return null;
            }

            var x = o.Select(or => new OwnerRequest
            {
                Id = or.Id,
                FirstName = or.User.FirstName,
                LastName = or.User.LastName,
                Email = or.User.Email,
                PhoneNumber = or.User.PhoneNumber
            }).ToList();

            return x;
        }

        public IEnumerable<OwnerRequest> GetOwnersWithMaterials()
        {
            var o = this.dataContext.Owners
                .Include(a => a.User);
                //.Include(m => m.Materials);

            if (o == null)
            {
                return null;
            }

            var x = o.Select(or => new OwnerRequest
            {
                Id = or.Id,
                FirstName = or.User.FirstName,
                LastName = or.User.LastName,
                Email = or.User.Email,
                PhoneNumber = or.User.PhoneNumber,
                //Materials = or.Materials.Select(m => new MaterialRequest
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

                //}).ToList()
            }).ToList();

            return x;
        }

        public OwnerRequest GetOwnerWithMaterialsByEmail(EmailRequest emailOwner)
        {
            var o = this.dataContext.Owners
                    .Include(u => u.User)
                    //.Include(m => m.Materials)
                    //.ThenInclude(mt => mt.MaterialType)
                    //.Include(m => m.Materials)
                    //.ThenInclude(s => s.Status)
                    .FirstOrDefault(o => o.User.Email.ToLower() == emailOwner.Email);

            if (o == null)
            {
                return null;
            }

            var x = new OwnerRequest
            {
                Id = o.Id,
                FirstName = o.User.FirstName,
                LastName = o.User.LastName,
                Email = o.User.Email,
                PhoneNumber = o.User.PhoneNumber,
                //Materials = o.Materials.Select(m => new MaterialRequest
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
                //}).ToList()
            };

            return x;
        }

        public IEnumerable<OwnerRequest> GetOwnersWithMaterialsByName(string name)
        {
            var o = this.dataContext.Owners
                    .Include(u => u.User)
                    //.Include(m => m.Materials)
                    //.ThenInclude(mt => mt.MaterialType)
                    //.Include(m => m.Materials)
                    //.ThenInclude(s => s.Status)
                    .Where(n => n.User.FirstName == name);

            if (o == null)
            {
                return null;
            }

            var x = o.Select(or => new OwnerRequest
            {
                Id = or.Id,
                FirstName = or.User.FirstName,
                LastName = or.User.LastName,
                Email = or.User.Email,
                PhoneNumber = or.User.PhoneNumber,
                //Materials = or.Materials.Select(m => new MaterialRequest
                //{
                //    Id = m.Id,
                //    Brand = m.Brand,
                //    Label = m.Label,
                //    MaterialModel = m.MaterialModel,
                //    MaterialType = m.MaterialType.Name,
                //    Name = m.Name,
                //    SerialNum = m.SerialNum,
                //    Status = m.Status.Name,
                //    Owner = m.Owner.User.FirstName
                //}).ToList()
            }).ToList();

            return x;
        }

        public OwnerRequest GetOwnerWithMaterialsById(int id)
        {
            var o = this.dataContext.Owners
                    .Include(u => u.User)
                    //.Include(m => m.Materials)
                    //.ThenInclude(mt => mt.MaterialType)
                    //.Include(m => m.Materials)
                    //.ThenInclude(s => s.Status)
                    .FirstOrDefault(o => o.Id == id);

            if (o == null)
            {
                return null;
            }

            var x = new OwnerRequest
            {
                Id = o.Id,
                FirstName = o.User.FirstName,
                LastName = o.User.LastName,
                Email = o.User.Email,
                PhoneNumber = o.User.PhoneNumber,
                //Materials = o.Materials.Select(m => new MaterialRequest
                //{
                //    Id = m.Id,
                //    Brand = m.Brand,
                //    Label = m.Label,
                //    MaterialModel = m.MaterialModel,
                //    MaterialType = m.MaterialType.Name,
                //    Name = m.Name,
                //    SerialNum = m.SerialNum,
                //    Status = m.Status.Name,
                //    Owner = m.Owner.User.FirstName
                //}).ToList()
            };

            return x;
        }
        public Owner GetOwnerByName(string name)
        {
            var a = this.dataContext.Users;
            var b = this.dataContext.Owners.Include(o => o.User);
            foreach (Entities.User u  in a)
            {
                if (u.FullName == name)
                {
                    foreach(Owner ow in b)
                    {
                        if(ow.User == u)
                        {
                            return ow;
                        }
                    }
                }
            }
            return null;
            //var a = this.dataContext.Users;
            //var si = new Entities.User();
            //foreach (MAV.Web.Data.Entities.User s in a)
            //{
            //    if (s.FirstName == name)
            //    {

            //        si = s;
            //        continue;
            //    }
            //}
            //if (si == null)
            //    return null;
            //else
            //{
            //    var ow = this.dataContext.Owners;
            //    var x = new Owner();
            //    foreach (Owner owner in ow)
            //    {
            //        if (owner.User == si)
            //        {

            //            x = owner;
            //            continue;
            //        }
            //    }
            //    return (x);
        }
    }
}


