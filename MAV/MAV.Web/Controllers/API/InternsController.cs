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
    public class InternsController:Controller
    {
        private readonly IInternRepository internRepository;
        private readonly IUserHelper userHelper;
        public InternsController(IInternRepository internRepository, IUserHelper userHelper)
        {
            this.internRepository = internRepository;
            this.userHelper = userHelper;
        }

        [HttpGet]
        public IActionResult GetInterns()
        {
            return Ok(this.internRepository.GetInterns());
            //return Ok(this.internRepository.GetInternsWithUser());
            //return Ok(this.internRepository.GetInternsWithLoansLoanDetailsWithMaterialAndOwner());
            //var emailIntern = new EmailRequest { Email = "deivi.hr@gmail.com" };
            //return Ok(this.internRepository.GetInternWithLoansLoanDetailsWithMaterialAndOwnerByEmail(emailIntern));
            //return Ok(this.internRepository.GetInternWithLoansByEmail(emailIntern));
            //return Ok(this.internRepository.GetInternsWithLoansLoanDetailsWithMaterialAndOwnerByName("Arturo"));
            //return Ok(this.internRepository.GetInternsWithLoansLoanDetailsWithMaterialAndOwnerById(1));
        }
        [HttpPost]
        public async Task<IActionResult> PostIntern([FromBody] InternRequest intern)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await this.userHelper.GetUserByEmailAsync(intern.Email);
            if (user == null)
            {
                user = new Data.Entities.User
                {
                    FirstName = intern.FirstName,
                    LastName = intern.LastName,
                    Email = intern.Email,
                    UserName = intern.Email,
                    PhoneNumber = intern.PhoneNumber
                };

                var result = await this.userHelper.AddUserAsync(user, intern.Password);

                if (result != IdentityResult.Success)
                {
                    return BadRequest("No se puede crear el usuario en la base de datos");
                }
                await this.userHelper.AddUserToRoleAsync(user, "Intern");
            }

            var emailIntern = new EmailRequest { Email = intern.Email };
            var oldIntern = this.internRepository.GetInternWithUserByEmail(emailIntern);
            if (oldIntern != null)
            {
                return BadRequest("Ya existe el usuario");
            }

            var entityIntern = new Intern
            {
                User = user
            };

            var newIntern = await this.internRepository.CreateAsync(entityIntern);
            return Ok(newIntern);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntern([FromRoute] int id, [FromBody] InternRequest intern)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != intern.Id)
            {
                return BadRequest();
            }
            var oldIntern = await this.userHelper.GetUserByEmailAsync(intern.Email);
            if (oldIntern == null)
            {
                return BadRequest("Id not found");
            }

            oldIntern.FirstName = intern.FirstName;
            oldIntern.LastName = intern.LastName;
            oldIntern.Email = intern.Email;
            oldIntern.PhoneNumber = intern.PhoneNumber;

            if (intern.OldPassword != intern.Password)
            {
                var pass = await this.userHelper.ChangePasswordAsync(oldIntern, intern.OldPassword, intern.Password);
                if (!pass.Succeeded)
                {
                    return BadRequest("No coincide la contraseña anterior, intente de nuevo");
                }
            }
            var result = await this.userHelper.UpdateUserAsync(oldIntern);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntern([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var oldIntern = await this.internRepository.GetByIdAsync(id);
            if (oldIntern == null)
            {
                return BadRequest("Id not found");
            }

            await this.internRepository.DeleteAsync(oldIntern);
            return Ok(oldIntern);
        }
    }
}
