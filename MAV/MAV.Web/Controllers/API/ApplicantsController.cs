namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApplicantsController:Controller
    {
        private readonly IApplicantRepository applicantRepository;

        public ApplicantsController(IApplicantRepository applicantRepository)
        {
            this.applicantRepository = applicantRepository;
        }

        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok(this.applicantRepository.GetApplicantsWithUser());
        }
        [HttpPost]
        public async Task<IActionResult> PostApplicant([FromBody] MAV.Common.Models.Applicant applicant)
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
