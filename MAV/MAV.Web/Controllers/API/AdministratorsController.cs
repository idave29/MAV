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

            var user = await this.userHelper.GetUserByEmailAsync(administrator.Email);
            if (user == null)
            {
                user = new Data.Entities.User
                {
                    FirstName = administrator.FirstName,
                    LastName = administrator.LastName,
                    Email = administrator.Email,
                    UserName = administrator.Email,
                    PhoneNumber = administrator.PhoneNumber
                };

                var result = await this.userHelper.AddUserAsync(user, administrator.Password);

                if (result != IdentityResult.Success)
                {
                    return BadRequest("No se puede crear el usuario en la base de datos");
                }
                await this.userHelper.AddUserToRoleAsync(user, "Admininstrator");
            }

            var emailAdmin = new EmailRequest { Email = administrator.Email };
            var oldAdministrator = this.administratorRepository.GetAdministratorWithUserByEmail(emailAdmin);
            if (oldAdministrator != null)
            {
                return BadRequest("Ya existe el usuario");
            }

            var entityAdministrators = new Administrator
            {
                User = user
            };

            var newAdministrator = await this.administratorRepository.CreateAsync(entityAdministrators);
            return Ok(newAdministrator);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdministrator([FromRoute] int id, [FromBody] AdministratorRequest administrator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != administrator.Id)
            {
                return BadRequest();
            }
            var user = await this.userHelper.GetUserByEmailAsync(administrator.Email);
            if (user == null)
            {
                return BadRequest("Id not found");
            }

            user.FirstName = administrator.FirstName;
            user.LastName = administrator.LastName;
            user.Email = administrator.Email;
            user.PhoneNumber = administrator.PhoneNumber;
            
            if (administrator.OldPassword != administrator.Password)
            {
                var pass = await this.userHelper.ChangePasswordAsync(user, administrator.OldPassword, administrator.Password);
                if (!pass.Succeeded)
                {
                    return BadRequest("No coincide la contraseña anterior, intente de nuevo");
                }
            }
            //else
            //    return BadRequest("Es la misma contraseña que la anterior");

            var result = await this.userHelper.UpdateUserAsync(user);
            
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdministrator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var oldAdministrator = await this.administratorRepository.GetByIdAsync(id);
            
            if (oldAdministrator == null)
            {
                return BadRequest("Id not found");
            }
            ;
            await this.administratorRepository.DeleteAsync(oldAdministrator);
            return Ok(oldAdministrator);

        }
    }
}
