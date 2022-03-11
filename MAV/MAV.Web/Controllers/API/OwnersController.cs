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

            var user = await this.userHelper.GetUserByEmailAsync(ownerRequest.Email);
            if (user == null)
            {
                user = new Data.Entities.User
                {
                    FirstName = ownerRequest.FirstName,
                    LastName = ownerRequest.LastName,
                    Email = ownerRequest.Email,
                    UserName = ownerRequest.Email,
                    PhoneNumber = ownerRequest.PhoneNumber
                };

                var result = await this.userHelper.AddUserAsync(user, ownerRequest.Password);

                if (result != IdentityResult.Success)
                {
                    return BadRequest("No se puede crear el usuario en la base de datos");
                }
                await this.userHelper.AddUserToRoleAsync(user, "Owner");
            }

            var emailOwner = new EmailRequest { Email = ownerRequest.Email };
            var oldOwner = this.ownerRepository.GetOwnerWithMaterialsByEmail(emailOwner);
            if (oldOwner != null)
            {
                return BadRequest("Ya existe el usuario");
            }

            var entityOwner = new Owner
            {
                User = user
            };

            var newOwner = await this.ownerRepository.CreateAsync(entityOwner);
            return Ok(newOwner);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOwner([FromRoute] int id, [FromBody] OwnerRequest owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != owner.Id)
            {
                return BadRequest();
            }
            var oldOwner = await this.ownerRepository.GetByIdAsync(id);
            if (oldOwner == null)
            {
                return BadRequest("Id not found");
            }

            oldOwner.User.FirstName = owner.FirstName;
            oldOwner.User.LastName = owner.LastName;
            oldOwner.User.Email = owner.Email;
            oldOwner.User.PhoneNumber = owner.PhoneNumber;

            var updateOwner = await this.ownerRepository.UpdateAsync(oldOwner);
            return Ok(updateOwner);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var oldOwner = await this.ownerRepository.GetByIdAsync(id);
            if (oldOwner == null)
            {
                return BadRequest("Id not found");
            }

            await this.ownerRepository.DeleteAsync(oldOwner);
            return Ok(oldOwner);
        }
    }
}
