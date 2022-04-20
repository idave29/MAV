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


            var user = await userHelper.GetUserByEmailAsync(applicant.Email);

            //var admin = this.administratorRepository.GetByIdAdministrator(administrator.Id);

            if (user == null)
            {
                return BadRequest("Not valid applicant.");
            }

            foreach (Applicant adminTemp in applicantRepository.GetApplicantsWithUser())
            {
                if (adminTemp.User == user)
                {
                    return BadRequest("Ya existe este solicitante");
                }
            }

            var entityAdministrator = new MAV.Web.Data.Entities.Applicant
            {

                User = user
            };

            await userHelper.AddUserToRoleAsync(user, "Solicitante");
            //if (entityMaterial == null)
            //{
            //    return BadRequest("entityMaterial not found");
            //}
            //var newMaterial = await this.materialRepository.CreateAsync(entityMaterial);
            var newAdmin = await this.applicantRepository.CreateAsync(entityAdministrator);

            return Ok(newAdmin);
        }     
    }
}
