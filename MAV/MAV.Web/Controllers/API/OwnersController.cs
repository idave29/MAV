namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Mvc;
    [Route("api/[Controller]")]
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
