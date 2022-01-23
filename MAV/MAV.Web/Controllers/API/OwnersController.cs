namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository ownerRepository;

        public OwnersController(IOwnerRepository ownerRepository)
        {
            this.ownerRepository = ownerRepository;
        }

        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok(this.ownerRepository.GetOwnersWithUser());
        }
    }
}
