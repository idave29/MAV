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


            var user = await userHelper.GetUserByEmailAsync(intern.Email);

            //var admin = this.administratorRepository.GetByIdAdministrator(administrator.Id);

            if (user == null)
            {
                return BadRequest("Not valid intern.");
            }

            foreach (Intern adminTemp in internRepository.GetInternsWithUser())
            {
                if (adminTemp.User == user)
                {
                    return BadRequest("Ya existe este becario");
                }
            }

            var entityAdministrator = new MAV.Web.Data.Entities.Intern
            {

                User = user
            };

            await userHelper.AddUserToRoleAsync(user, "Becario");
            //if (entityMaterial == null)
            //{
            //    return BadRequest("entityMaterial not found");
            //}
            //var newMaterial = await this.materialRepository.CreateAsync(entityMaterial);
            var newAdmin = await this.internRepository.CreateAsync(entityAdministrator);

            return Ok(newAdmin);
        }
    }
}
