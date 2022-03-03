namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
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
        public IActionResult GetOwners()
        {
            //return Ok(this.ownerRepository.GetOwnersWithUser());
            //return Ok(this.ownerRepository.GetOwnersWithMaterials());
            return Ok(this.ownerRepository.GetOwners());
            //var emailOwner = new EmailRequest { Email = "keanu.reeves@gmail.com" };
            //return Ok(this.ownerRepository.GetOwnerWithMaterialsByEmail(emailOwner));
            //return Ok(this.ownerRepository.GetOwnersWithMaterialsByName("Carlos"));
            //return Ok(this.ownerRepository.GetOwnerWithMaterialsById(1));

        }
    }
}
