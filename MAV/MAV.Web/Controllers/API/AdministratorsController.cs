namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using MAV.Web.Data.Repositories;
    using MAV.Web.Helpers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdministratorsController:Controller
    {
        private readonly IAdministratorRepository administratorRepository;
        private readonly IUserHelper userHelper; 
        public AdministratorsController(IAdministratorRepository administratorRepository, IUserHelper userHelper)
        {
            this.administratorRepository = administratorRepository;
            this.userHelper = userHelper;   
        }

        [HttpGet]
        public IActionResult GetAdministrators()
        {
            //return Ok(this.administratorRepository.GetAdministratorsWithUserByName("Miguel"));
            //var emailApplicant = new EmailRequest { Email = "miguel.ochoa@gmail.com" };
            //return Ok(this.administratorRepository.GetAdministratorWithUserByEmail(emailApplicant));
            //return Ok(this.administratorRepository.GetAdministratorWithUserById(1));
            //return Ok(this.administratorRepository.GetAdministratorsWithUser());
            return Ok(this.administratorRepository.GetAdministrators());
        }

        [HttpPost]
        public async Task<IActionResult> PostAdministrator([FromBody] AdministratorRequest administrator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var user = await userHelper.GetUserByEmailAsync(administrator.Email);

            //var admin = this.administratorRepository.GetByIdAdministrator(administrator.Id);

            if (user == null)
            {
                return BadRequest("Not valid admin.");
            }

            foreach (Administrator adminTemp in administratorRepository.GetAdministratorsWithUser())
            {
                if (adminTemp.User == user)
                {
                    return BadRequest("Ya existe este admin");
                }
            }

            var entityAdministrator = new MAV.Web.Data.Entities.Administrator
            {

                User = user
            };

            await userHelper.AddUserToRoleAsync(user, "Administrador");
            //if (entityMaterial == null)
            //{
            //    return BadRequest("entityMaterial not found");
            //}
            //var newMaterial = await this.materialRepository.CreateAsync(entityMaterial);
            var newAdmin = await this.administratorRepository.CreateAsync(entityAdministrator);

            return Ok(newAdmin);



            //var user = await this.userHelper.GetUserByEmailAsync(administrator.Email);
            //if (user == null)
            //{
            //    user = new Data.Entities.User
            //    {
            //        FirstName = administrator.FirstName,
            //        LastName = administrator.LastName,
            //        Email = administrator.Email,
            //        UserName = administrator.Email,
            //        PhoneNumber = administrator.PhoneNumber
            //    };

            //    var result = await this.userHelper.AddUserAsync(user, administrator.Password);

            //    if (result != IdentityResult.Success)
            //    {
            //        return BadRequest("No se puede crear el usuario en la base de datos");
            //    }
            //    await this.userHelper.AddUserToRoleAsync(user, "Administrador");
            //}

            //var emailAdmin = new EmailRequest { Email = administrator.Email };
            //var oldAdministrator = this.administratorRepository.GetAdministratorWithUserByEmail(emailAdmin);
            //if (oldAdministrator != null)
            //{
            //    return BadRequest("Ya existe el usuario");
            //}

            //var entityAdministrators = new Administrator
            //{
            //    User = user
            //};

            //var newAdministrator = await this.administratorRepository.CreateAsync(entityAdministrators);
            //return Ok(newAdministrator);
        }
    }
}
