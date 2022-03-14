namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
    using MAV.Web.Data.Repositories;
    using MAV.Web.Helpers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApplicantsController:Controller
    {
        private readonly IApplicantRepository applicantRepository;
        private readonly IApplicantTypeRepository applicantTypeRepository;
        private readonly IUserHelper userHelper;

        public ApplicantsController(IApplicantRepository applicantRepository, IApplicantTypeRepository applicantTypeRepository, IUserHelper userHelper)
        {
            this.applicantRepository = applicantRepository;
            this.applicantTypeRepository = applicantTypeRepository;
            this.userHelper = userHelper;
        }

        [HttpGet]
        public IActionResult GetApplicants()
        {
            return Ok(this.applicantRepository.GetApplicants());
            //return Ok(this.applicantRepository.GetApplicantsWithUser());
            //var emailApplicant = new EmailRequest { Email = "raquel.sanchez@gmail.com" };
            //return Ok(this.applicantRepository.GetApplicantWithInternLoanLoanDetailsMaterialAndOwnerByEmail(emailApplicant));
            //return Ok(this.applicantRepository.GetApplicantsWithInternLoanLoanDetailsMaterialAndOwnerByNameApplicant("Natalia"));
            //return Ok(this.applicantRepository.GetApplicantsWithInternLoanLoanDetailsMaterialAndOwnerByApplicantType("No Deudor"));
            //return Ok(this.applicantRepository.GetApplicantsWithInternLoanLoanDetailsMaterialAndOwner());
            //return Ok(this.applicantRepository.GetByIdApplicantWithUser(1));
        }

        [HttpPost]
        public async Task<IActionResult> PostApplicant([FromBody] MAV.Common.Models.ApplicantRequest applicant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            var applicantType = this.applicantTypeRepository.GetApplicantTypeByName(applicant.ApplicantType);
            if (applicantType == null)
            {
                return BadRequest("applicantType not found");
            }

            var user = await this.userHelper.GetUserByEmailAsync(applicant.Email);
            if (user == null)
            {
                user = new Data.Entities.User
                {
                    FirstName = applicant.FirstName,
                    LastName = applicant.LastName,
                    Email = applicant.Email,
                    UserName = applicant.Email,
                    PhoneNumber = applicant.PhoneNumber
                };

                var result = await this.userHelper.AddUserAsync(user, applicant.Password);

                if (result != IdentityResult.Success)
                {
                    return BadRequest("No se puede crear el usuario en la base de datos");
                }
                await this.userHelper.AddUserToRoleAsync(user, "Applicant");
            }
            //var emailApplicant = new EmailRequest { Email = applicant.Email };
            //var oldApplicant = this.applicantRepository.GetApplicantWithInternLoanLoanDetailsMaterialAndOwnerByEmail(emailApplicant);
            //if (oldApplicant != null)
            //{
            //    return BadRequest("Ya existe el usuario");
            //}
            var entityApplicant = new MAV.Web.Data.Entities.Applicant
            {
                Id = applicant.Id,
                User = user,
                ApplicantType= applicantType

            };
            if (entityApplicant == null)
            {
                return BadRequest("entityApplicant not found");
            }
            var newApplicant = await this.applicantRepository.CreateAsync(entityApplicant);
            return Ok(newApplicant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicant([FromRoute] int id, [FromBody] ApplicantRequest applicant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != applicant.Id)
            {
                return BadRequest();
            }
            var user = await this.userHelper.GetUserByEmailAsync(applicant.Email);
            if (user == null)
            {
                return BadRequest("Id not found");
            }

            user.FirstName = applicant.FirstName;
            user.LastName = applicant.LastName;
            user.Email = applicant.Email;
            user.PhoneNumber = applicant.PhoneNumber;

            if (applicant.OldPassword != applicant.Password)
            {
                var pass = await this.userHelper.ChangePasswordAsync(user, applicant.OldPassword, applicant.Password);
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
        public async Task<IActionResult> DeleteApplicant([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var oldApplicant = await this.applicantRepository.GetByIdAsync(id);

            if (oldApplicant == null)
            {
                return BadRequest("Id not found");
            }
            ;
            await this.applicantRepository.DeleteAsync(oldApplicant);
            return Ok(oldApplicant);

        }
    }
}
