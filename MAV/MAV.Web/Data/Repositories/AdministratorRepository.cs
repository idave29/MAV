namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AdministratorRepository : GenericRepository<Administrator>, IAdministratorRepository
    {
        private readonly DataContext dataContext;

        public AdministratorRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboAdministrators()
        {
            var list = this.dataContext.Administrators.Select(t => new SelectListItem
            {
                Text = t.User.FullName,
                Value = $"{t.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un administrador...)",
                Value = "0"
            });
            return list;
        }

        public IQueryable GetAdministratorsWithUser()
        {
            return this.dataContext.Administrators
                .Include(t => t.User);
        }

        public IEnumerable<AdministratorRequest> GetAdministrators()
        {
            var a = this.dataContext.Administrators;

            if (a == null)
            {
                return null;
            }

            var x = a.Select(ar => new AdministratorRequest
            {
                Id = ar.Id,
                FirstName = ar.User.FirstName,
                LastName = ar.User.LastName,
                PhoneNumber = ar.User.PhoneNumber,
                Email = ar.User.Email,
                //Password = ar.User.PasswordHash
            }).ToList();

            return x;
        }

    public async Task<Administrator> GetByIdWithUserAsync(int id)
        {
            return await this.dataContext.Administrators
                .Include(t => t.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Administrator> GetByIdUserWithUserAdminAsync(string id)
        {
            return await this.dataContext.Administrators
                .Include(t => t.User)
                .FirstOrDefaultAsync(e => e.User.Id == id);

            //if (a == null)
            //{
            //    return null;
            //}
            //var x = new AdministratorRequest
            //{
            //    Id = a.Id,
            //    Email = a.User.Email,
            //    FirstName = a.User.FirstName,
            //    LastName = a.User.LastName,
            //    PhoneNumber = a.User.PhoneNumber
            //};

            //return a;
        }

        public AdministratorRequest GetAdministratorWithUserByEmail(EmailRequest email)
        {
            var a = this.dataContext.Administrators
               .Include(t => t.User)
               .FirstOrDefault(u => u.User.Email.ToLower() == email.ToString());

            if (a == null)
            {
                return null;
            }
            var x = new AdministratorRequest
            {
                Id = a.Id,
                Email = a.User.Email,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                PhoneNumber = a.User.PhoneNumber
            };

            return x;
        }

        public IEnumerable<AdministratorRequest> GetAdministratorsWithUserByName(string name)
        {
            var a = this.dataContext.Administrators;

            if (a == null)
            {
                return null;
            }

            var x = a.Select(ar => new AdministratorRequest
            {
                Id = ar.Id,
                FirstName = ar.User.FirstName,
                LastName = ar.User.LastName,
                PhoneNumber = ar.User.PhoneNumber,
                Email = ar.User.Email

            }).ToList().Where(n => n.FirstName == name);

            return x;
        }
    }
}
