namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using MAV.Web.Data.Repositories;
    using MAV.Web.Helpers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository ownerRepository;
        private readonly IUserHelper userHelper;

        public OwnersController(IOwnerRepository ownerRepository, IUserHelper userHelper)
        {
            this.ownerRepository = ownerRepository;
            this.userHelper = userHelper;
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

        [HttpPost]
        public async Task<IActionResult> PostOwner([FromBody] OwnerRequest ownerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var user = await userHelper.GetUserByEmailAsync(ownerRequest.Email);

            //var admin = this.administratorRepository.GetByIdAdministrator(administrator.Id);

            if (user == null)
            {
                return BadRequest("Not valid owner.");
            }

            foreach (Owner adminTemp in ownerRepository.GetOwnersWithUser())
            {
                if (adminTemp.User == user)
                {
                    return BadRequest("Ya existe este responsable");
                }
            }

            var entityAdministrator = new MAV.Web.Data.Entities.Owner
            {

                User = user
            };

            await userHelper.AddUserToRoleAsync(user, "Responsable");
            //if (entityMaterial == null)
            //{
            //    return BadRequest("entityMaterial not found");
            //}
            //var newMaterial = await this.materialRepository.CreateAsync(entityMaterial);
            var newAdmin = await this.ownerRepository.CreateAsync(entityAdministrator);

            return Ok(newAdmin);

        }
    }
}
