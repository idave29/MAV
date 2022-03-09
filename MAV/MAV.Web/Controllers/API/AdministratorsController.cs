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
            //var entityAdministrators = new Administrator
            //{
            //    User = new Data.Entities.User
            //    {
            //        FirstName = administrator.FirstName,
            //        LastName = administrator.LastName,
            //        Email = administrator.Email,
            //        PhoneNumber = administrator.PhoneNumber,
            //        PasswordHash = administrator.Password                    
            //    }
            //};
            //var newAdministrator = await this.administratorRepository.CreateAsync(entityAdministrators);
            //return Ok(newAdministrator);



            //var user = await this.userHelper.GetUserByEmailAsync(administrator.Email);
            //if(user != null)
            //{
            //    return BadRequest(ModelState);
            //}
            var entityAdministrators = new Administrator
            {
                User = new Data.Entities.User
                {
                    FirstName = administrator.FirstName,
                    LastName = administrator.LastName,
                    Email = administrator.Email,
                    PhoneNumber = administrator.PhoneNumber,
                    UserName = administrator.Email
                }
            };

            //var result = await this.userHelper.AddUserAsync(user, administrator.Password);
            //if (result != IdentityResult.Success)
            //{
            //    throw new InvalidOperationException("No se puede crear el usuario en la base de datos");
            //}
            //await userHelper.AddUserToRoleAsync(user, administrator.Role);


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
            var oldAdministrator = await this.administratorRepository.GetByIdAsync(id);
            if (oldAdministrator == null)
            {
                return BadRequest("Id not found");
            }

            oldAdministrator.User.FirstName = administrator.FirstName;
            oldAdministrator.User.LastName = administrator.LastName;
            oldAdministrator.User.Email = administrator.Email;
            oldAdministrator.User.PhoneNumber = administrator.PhoneNumber;

            var updateMaterialType = await this.administratorRepository.UpdateAsync(oldAdministrator);
            return Ok(updateMaterialType);
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

            await this.administratorRepository.DeleteAsync(oldAdministrator);
            return Ok(oldAdministrator);
        }
    }
}
