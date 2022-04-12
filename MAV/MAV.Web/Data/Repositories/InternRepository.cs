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

        public async Task<Intern> GetByIdUserInternWithUserAsync(string id)
        {
            return await this.dataContext.Interns
                .Include(t => t.User)
                .FirstOrDefaultAsync(e => e.User.Id == id);
        }

        public async Task<Intern> GetByIdInternWithLoansAsync(int id)
        {
            return await this.dataContext.Interns
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Intern> GetByIdInternWithLoansLoansDetailsAsync(int id)
        {
            return await this.dataContext.Interns
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Intern> GetByIdWithUserAsync(int id)
        {
            return await this.dataContext.Interns
                 .Include(t => t.User)
                 .Where(a => a.User.Deleted == false)
                 .FirstOrDefaultAsync(e => e.Id == id);
        }
        public InternRequest GetInternWithUserById(int id)
        {
            var a = this.dataContext.Interns
               .FirstOrDefault(u => u.Id == id);

            if (a == null)
            {
                return null;
            }
            var x = new InternRequest
            {
                Id = a.Id,
                Email = a.User.Email,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                PhoneNumber = a.User.PhoneNumber
            };

            return x;
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
                .Include(i => i.User)
                .Where(a => a.User.Deleted == false);
        }

        public IEnumerable<InternRequest> GetInterns()
        {
            var i = this.dataContext.Interns
                    .Where(a => a.User.Deleted == false);

            if (i == null)
            {
                return null;
            }

            var x = i.Select(ar => new InternRequest
            {
                Id = ar.Id,
                FirstName = ar.User.FirstName,
                LastName = ar.User.LastName,
                PhoneNumber = ar.User.PhoneNumber,
                Email = ar.User.Email,
            }).ToList();

            return x;

        }


        public IEnumerable<InternRequest> GetInternsWithLoansLoanDetailsWithMaterialAndOwner()
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
                PhoneNumber = a.User.PhoneNumber,
                
            }).ToList();

            return x;
        }

        public InternRequest GetInternWithLoansLoanDetailsWithMaterialAndOwnerByEmail(EmailRequest emailIntern)
        {
            var i = this.dataContext.Interns
                .Include(i => i.User)
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
            };

            return x;
        }

        public InternRequest GetInternWithLoansByEmail(EmailRequest emailIntern)
        {
            var i = this.dataContext.Interns
                .Include(i => i.User)
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
                PhoneNumber = i.User.PhoneNumber
                
             };

            return x;
        }

        public IEnumerable<InternRequest> GetInternsWithLoansLoanDetailsWithMaterialAndOwnerByName(string name)
        {
            var i = this.dataContext.Interns
                .Include(i => i.User)
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
                
            }).ToList();

            return x;
        }

        public IEnumerable<InternRequest> GetInternsWithLoansLoanDetailsWithMaterialAndOwnerById(int id)
        {
            var i = this.dataContext.Interns
                .Include(i => i.User)
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
                
            }).ToList();

            return x;
        }
        public InternRequest GetInternWithUserByEmail(EmailRequest email)
        {
            var a = this.dataContext.Interns
               .Include(t => t.User)
               .Where(a => a.User.Deleted == false)
               .FirstOrDefault(u => u.User.Email.ToLower() == email.ToString());

            if (a == null)
            {
                return null;
            }
            var x = new InternRequest
            {
                Id = a.Id,
                Email = a.User.Email,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                PhoneNumber = a.User.PhoneNumber
            };

            return x;
        }
        public IEnumerable<InternRequest> GetInternsWithUserByName(string name)
        {
            var a = this.dataContext.Interns;

            if (a == null)
            {
                return null;
            }

            var x = a.Select(ar => new InternRequest
            {
                Id = ar.Id,
                FirstName = ar.User.FirstName,
                LastName = ar.User.LastName,
                PhoneNumber = ar.User.PhoneNumber,
                Email = ar.User.Email

            }).ToList().Where(n => n.FirstName == name);

            return x;
        }
        public Intern GetInternByName(string fullname)
        {
            //var a = this.dataContext.Interns.Include(t => t.User);
            //foreach (Intern i in a)
            //{
            //    if (i.User.FullName == fullname)
            //    {
            //        return i;
            //    }
            //}
            var a = this.dataContext.Users.Where(a => a.Deleted == false);
            var b = this.dataContext.Interns.Where(a => a.User.Deleted == false).Include(o => o.User);
            foreach (Entities.User u in a)
            {
                if (u.FullName == fullname)
                {
                    foreach (Intern i in b)
                    {
                        if (i.User == u)
                        {
                            return i;
                        }
                    }
                }
            }
            return null;
        }
    }
}
