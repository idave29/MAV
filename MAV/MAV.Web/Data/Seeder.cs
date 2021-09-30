using MAV.Web.Data.Entities;
using MAV.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
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
            //User
            await dataContext.Database.EnsureCreatedAsync();
            await this.userHelper.CheckRoleAsync("Admininstrator");
            await this.userHelper.CheckRoleAsync("Applicant");
            await this.userHelper.CheckRoleAsync("Intern");
            await this.userHelper.CheckRoleAsync("Owner");
            //Roles
            if (!this.dataContext.Administrators.Any())
            {
                var user = await CheckUserAsync("Fong", "Eduardo", "eduardo.fong@gmail.com", "987654321", "123456", "Admininstrator");
                await CheckAdminAsync(user);
            }
            if (!this.dataContext.Applicants.Any())
            {
                var user = await CheckUserAsync("Zambrano", "Natalia", "natalia.xambrano@gmail.com", "987654321", "123456", "Applicant");
                await CheckApplicantAsync(user);
            }
            if (!this.dataContext.Interns.Any())
            {
                var user = await CheckUserAsync("Hernandez", "David", "deivi.hr@gmail.com", "987654321", "123456", "Intern");
                await CheckInternAsync(user);
            }
            if (!this.dataContext.Owners.Any())
            {
                var user = await CheckUserAsync("Reeves", "Keanu", "keanu.reeves@gmail.com", "987654321", "123456", "Owner");
                await CheckOwnerAsync(user);
            }
            //ApplicantTypes
            if (!this.dataContext.ApplicantTypes.Any())
            {
                await CheckApplicantTypeAsync("No deudor");
            }
            //MaterialType
            if (!this.dataContext.MaterialTypes.Any())
            {
                await CheckMaterialTypeAsync("Cable");
            }
            //Status
            if (!this.dataContext.Statuses.Any())
            {
                await CheckStatusAsync("Disponible");
            }
            //Loan
            if (!this.dataContext.Loans.Any())
            {
                var applicant = this.dataContext.Applicants
                    .Include(c => c.User)
                    .FirstOrDefault(c => c.User.FirstName == "Natalia");
                var intern = this.dataContext.Interns
                    .Include(c => c.User)
                    .FirstOrDefault(c => c.User.FirstName == "David");
                await CheckLoanAsync(applicant,intern);
            }
            //Material
            if (!this.dataContext.Materials.Any())
            {
                var owner = this.dataContext.Owners
                    .Include(c => c.User)
                    .FirstOrDefault(c => c.User.FirstName == "Keanu");
                var status = this.dataContext.Statuses
                    .FirstOrDefault(s => s.Name == "Disponible");
                var materialType = this.dataContext.MaterialTypes
                    .FirstOrDefault(s => s.Name == "Cable");
                await this.CheckMaterialAsync("HDMI1",owner, materialType, status, "Sony", "MAV01", "Azul","897654");
            }
            //LoanDetail
            if (!this.dataContext.LoanDetails.Any())
            {
                var loan = this.dataContext.Loans.FirstOrDefault();
                var material = this.dataContext.Materials.FirstOrDefault();
                await CheckLoanDetailAsync(loan, material);
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
        private async Task CheckApplicantTypeAsync(string name)
        {
            this.dataContext.ApplicantTypes.Add(new ApplicantType { Name = name });
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


        private async Task CheckLoanAsync(Applicant applicant, Intern intern)
        {
            this.dataContext.Loans.Add(new Loan
            {
                Applicant = applicant,
                Intern = intern
            });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckLoanDetailAsync(Loan loan, Material material)
        {
            this.dataContext.LoanDetails.Add(new LoanDetail
            {
                Loan = loan,
                Material = material
            });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckMaterialAsync(string name, Owner owner, MaterialType materialType, Status status, string brand, string label, string materialModel, string serialNum)
        {
            this.dataContext.Materials.Add(new Material
            {
                Name = name,
                Owner = owner,
                Status = status,
                MaterialType = materialType,
                Brand = brand,
                Label = label,
                MaterialModel = materialModel,
                SerialNum = serialNum
            });
            await this.dataContext.SaveChangesAsync();
        }

    }
}
