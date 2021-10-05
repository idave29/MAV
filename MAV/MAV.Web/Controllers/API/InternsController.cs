namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Mvc;
    [Route("api/[Controller]")]
    public class InternsController:Controller
    {
        private readonly IInternRepository internRepository;

        public InternsController(IInternRepository internRepository)
        {
            this.internRepository = internRepository;
        }

        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok(this.internRepository.GetInternsWithUser());
        }
    }
}
