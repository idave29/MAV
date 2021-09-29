namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Mvc;
    [Route("api/[Controller]")]
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
