namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme) ]
    public class ApplicantTypesController : Controller
    {
        private readonly IApplicantTypeRepository applicantTypeRepository;

        public ApplicantTypesController(IApplicantTypeRepository applicantTypeRepository)
        {
            this.applicantTypeRepository = applicantTypeRepository;
        }

        [HttpGet]
        public IActionResult GetApplicantTypes()
        {
            return Ok(this.applicantTypeRepository.GetAll());
        }
    }
}
