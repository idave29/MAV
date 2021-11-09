namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    [Route("api/[Controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme) ]
    public class StatusController : Controller
    {
        private readonly IStatusRepository statusRepository;

        public StatusController(IStatusRepository statusRepository)
        {
            this.statusRepository = statusRepository;
        }

        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok(this.statusRepository.GetAll());
        }
    }
}
