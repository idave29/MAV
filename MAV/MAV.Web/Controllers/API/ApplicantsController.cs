namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
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
    }
}
