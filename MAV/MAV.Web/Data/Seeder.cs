using MAV.Web.Data.Entities;
using MAV.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAV.Web.Data
{
    public class Seeder
    {
        private readonly DataContext dataContext;
        private readonly IUserHelper userHelper;

        public Seeder(DataContext dataContext, IUserHelper userHelper)
        {
            this.dataContext = dataContext;
            this.userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await dataContext.Database.EnsureCreatedAsync();
            await this.userHelper.CheckRoleAsync("Admin");
            await this.userHelper.CheckRoleAsync("Applicant");
            await this.userHelper.CheckRoleAsync("Intern");
            await this.userHelper.CheckRoleAsync("Owner");

            if (!this.dataContext.Administrators.Any())
            {
                var user = await CheckUserAsync("Fong", "Eduardo", "eduardo.fong@gmail.com", "987654321", "123456", "Admin");
                await CheckAdminAsync(user);
            }
            if (!this.dataContext.Applicants.Any())
            {
                var user = await CheckUserAsync("Zambrano", "Natalia", "natalia.xambrano@gmail.com", "987654321", "123456", "Applicant");
                await CheckApplicantAsync(user);
            }
            if (!this.dataContext.Interns.Any())
            {
                var user = await CheckUserAsync("Espitia", "Mauricio", "mauricio.espitia@gmail.com", "987654321", "123456", "Intern");
                await CheckInternAsync(user);
            }
            if (!this.dataContext.Owners.Any())
            {
                var user = await CheckUserAsync("Reeves", "Keanu", "keanu.reeves@gmail.com", "987654321", "123456", "Owner");
                await CheckOwnerAsync(user);
            }
        }

        private async Task<User> CheckUserAsync(string lastName, string firstName, string mail, string phone, string password, string rol)
        {
            var user = await userHelper.GetUserByEmailAsync(mail);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = mail,
                    UserName = mail,
                    PhoneNumber = phone,
                };

                var result = await userHelper.AddUserAsync(user, password);
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("No se puede crear el usuario en la base de datos");
                }
                await userHelper.AddUserToRoleAsync(user, rol);
            }
            return user;
        }

        private async Task CheckAdminAsync(User user)
        {
            this.dataContext.Administrators.Add(new Administrator { User = user });
            await this.dataContext.SaveChangesAsync();
        }
        private async Task CheckApplicantAsync(User user)
        {
            this.dataContext.Applicants.Add(new Applicant { User = user });
            await this.dataContext.SaveChangesAsync();
        }
        private async Task CheckInternAsync(User user)
        {
            this.dataContext.Interns.Add(new Intern 
            { 
                User = user,
            });
            await this.dataContext.SaveChangesAsync();
        }
        private async Task CheckOwnerAsync(User user)
        {
            this.dataContext.Owners.Add(new Owner { User = user });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckMaterialTypeAsync(string name)
        {
            this.dataContext.MaterialTypes.Add(new MaterialType { Name = name });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckStatusAsync(string name)
        {
            this.dataContext.Statuses.Add(new Status { Name = name });
            await this.dataContext.SaveChangesAsync();
        }
        private async Task CheckApplicantTypeAsync(string name)
        {
            this.dataContext.ApplicantTypes.Add(new ApplicantType { Name = name });
            await this.dataContext.SaveChangesAsync();
        }

    }
}
