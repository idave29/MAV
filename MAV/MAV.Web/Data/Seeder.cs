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

            //Roles
            await this.userHelper.CheckRoleAsync("Administrador");
            await this.userHelper.CheckRoleAsync("Becario");
            await this.userHelper.CheckRoleAsync("Solicitante");
            await this.userHelper.CheckRoleAsync("Responsable");

            //Administradores
            if (!this.dataContext.Administrators.Any())
            {
                var user = await CheckUserAsync("Hernández", "David", "david.hdezalv29@gmail.com", "2221975824", "123456", "Administrador");
                await CheckAdminAsync(user);
            }

            //Tipo de solicitantes
            if (!this.dataContext.ApplicantTypes.Any())
            {
                await CheckApplicantTypeAsync("Alumno");
                await CheckApplicantTypeAsync("Profesor");
                await CheckApplicantTypeAsync("Administrativo"); 
            }

            //Solicitantes
            if (!this.dataContext.Applicants.Any())
            {
                var user = await CheckUserAsync("García García", "Kevin Javier", "kevin.garcia@gmail.com", "2221815266", "123456", "Solicitante");
                var applicantType = this.dataContext.ApplicantTypes.FirstOrDefault();
                await CheckApplicantAsync(user, applicantType);
            }

            //Becarios
            if (!this.dataContext.Interns.Any())
            {
                var user = await CheckUserAsync("Hernández", "David", "isf7541@umad.edu.mx", "2221975824", "123456", "Becario");
                await CheckInternAsync(user);
                user = await CheckUserAsync("Villegas", "Arturo", "artur.vr@gmail.com", "2229074543", "123456", "Becario");
                await CheckInternAsync(user);
            }

            //Responsables
            if (!this.dataContext.Owners.Any())
            {
                var user = await CheckUserAsync("Ochoa", "Miguel", "miguel.ochoa@umad.edu.mx", "2225675423", "123456", "Responsable");
                await CheckOwnerAsync(user);
            }
            
            //Tipo de material
            if (!this.dataContext.MaterialTypes.Any())
            {
                await CheckMaterialTypeAsync("Cable");
                await CheckMaterialTypeAsync("Adaptador"); 
                await CheckMaterialTypeAsync("Bocinas");
            }

            //Status
            if (!this.dataContext.Statuses.Any())
            {
                await CheckStatusAsync("Disponible");
                await CheckStatusAsync("Prestado");
                await CheckStatusAsync("Regresado");
                await CheckStatusAsync("Defectuoso");
            }

            //Préstamos
            if (!this.dataContext.Loans.Any())
            {
                var applicant = this.dataContext.Applicants
                    .Include(c => c.User)
                    .FirstOrDefault(c => c.User.FirstName == "Kevin Javier");
                var intern = this.dataContext.Interns
                    .Include(c => c.User)
                    .FirstOrDefault(c => c.User.FirstName == "David");
                await CheckLoanAsync(applicant, intern);
            }

            //Material
            if (!this.dataContext.Materials.Any())
            {
                var owner = this.dataContext.Owners
                    .Include(c => c.User)
                    .FirstOrDefault(c => c.User.FirstName == "Miguel");
                var status = this.dataContext.Statuses
                    .FirstOrDefault(s => s.Name == "Disponible");
                var materialType = this.dataContext.MaterialTypes
                    .FirstOrDefault(s => s.Name == "Cable");
                await this.CheckMaterialAsync("HDMI1",owner, materialType, status, "Sony", "MAV01", "Azul","897654", "Manda video y audio");
                owner = this.dataContext.Owners
                    .Include(c => c.User)
                    .FirstOrDefault(c => c.User.FirstName == "Miguel");
                status = this.dataContext.Statuses
                    .FirstOrDefault(s => s.Name == "Disponible");
                materialType = this.dataContext.MaterialTypes
                    .FirstOrDefault(s => s.Name == "Cable");
                await this.CheckMaterialAsync("VGA", owner, materialType, status, "HP", "MAV02", "Verde", "6817654", "Solo manda video");
            }

            //Detalle del préstamo
            if (!this.dataContext.LoanDetails.Any())
            {
                var loan = this.dataContext.Loans.FirstOrDefault();
                var material = this.dataContext.Materials.FirstOrDefault();
                var status = this.dataContext.Statuses.FirstOrDefault(s => s.Id == 3); 
                var dateTimeIn = new DateTime(2021, 10, 5, 8, 18, 0);
                var dateTimeOut = new DateTime(2021, 10, 6, 8, 30, 0);
                await CheckLoanDetailAsync(loan, material, dateTimeIn, dateTimeOut, status, "");
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
                    Deleted = false
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
        private async Task CheckApplicantAsync(User user, ApplicantType applicantType)
        {
            this.dataContext.Applicants.Add(new Applicant 
            { User = user, 
              ApplicantType = applicantType
            });
            await this.dataContext.SaveChangesAsync();
        }
        private async Task CheckApplicantTypeAsync(string name)
        {
            this.dataContext.ApplicantTypes.Add(new ApplicantType 
            { Name = name });
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

        private async Task CheckLoanDetailAsync(Loan loan, Material material, DateTime dateTimeIn, DateTime dateTimeOut, Status status, string observations)
        {
            this.dataContext.LoanDetails.Add(new LoanDetail
            {
                Loan = loan,
                Material = material,
                DateTimeIn = dateTimeIn,
                DateTimeOut = dateTimeOut, 
                Status = status,
                Observations = observations
            });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckMaterialAsync(string name, Owner owner, MaterialType materialType, Status status, string brand, string label, string materialModel, string serialNum, string function)
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
                SerialNum = serialNum, 
                Function = function,
                Deleted = false
            });
            await this.dataContext.SaveChangesAsync();
        }

    }
}
