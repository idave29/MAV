namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApplicantsController:Controller
    {
        private readonly IApplicantRepository applicantRepository;

        public ApplicantsController(IApplicantRepository applicantRepository)
        {
            this.applicantRepository = applicantRepository;
        }

        [HttpGet]
        public IActionResult GetApplicants()
        {
            return Ok(this.applicantRepository.GetApplicantsWithUser());
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
            var entityApplicant = new MAV.Web.Data.Entities.Applicant
            {
                //User = 
            };
            var newApplicant = await this.applicantRepository.CreateAsync(entityApplicant);
            return Ok(newApplicant);
        }
    }
}
