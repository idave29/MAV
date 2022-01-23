namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
