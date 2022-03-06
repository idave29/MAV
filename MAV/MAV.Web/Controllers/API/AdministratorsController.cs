namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdministratorsController:Controller
    {
        private readonly IAdministratorRepository administratorRepository;

        public AdministratorsController(IAdministratorRepository administratorRepository)
        {
            this.administratorRepository = administratorRepository;
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
        public async Task<IActionResult> PostStatus([FromBody] AdministratorRequest administrator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entityAdministrators = new Administrator
            {
                User = new Data.Entities.User()
                {
                    FirstName = administrator.FirstName,
                    LastName = administrator.LastName,
                    Email = administrator.Email,
                    PhoneNumber = administrator.PhoneNumber,
                    PasswordHash = administrator.LastName
                }
            };
            var newAdministrator = await this.administratorRepository.CreateAsync(entityAdministrators);
            return Ok(newAdministrator);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterialType([FromRoute] int id, [FromBody] AdministratorRequest administrator)
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
        public async Task<IActionResult> DeleteMaterialType([FromRoute] int id)
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
