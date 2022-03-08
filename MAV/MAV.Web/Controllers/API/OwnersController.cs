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
            var entityOwner = new Owner
            {
                User = new Data.Entities.User()
                {
                    FirstName = ownerRequest.FirstName,
                    LastName = ownerRequest.LastName,
                    Email = ownerRequest.Email,
                    PhoneNumber = ownerRequest.PhoneNumber,
                    //PasswordHash = ownerRequest.LastName
                }
            };
            var newAdministrator = await this.ownerRepository.CreateAsync(entityOwner);
            return Ok(newAdministrator);
            //if (ModelState.IsValid)
            //{
            //    var user = await userHelper.GetUserByEmailAsync(ownerRequest.Email);
            //    if (user == null)
            //    {
            //        user = new Data.Entities.User
            //        {

            //            FirstName = ownerRequest.FirstName,
            //            LastName = ownerRequest.LastName,
            //            PhoneNumber = ownerRequest.PhoneNumber,
            //            Email = ownerRequest.Email,
            //            UserName = ownerRequest.Email
            //        };

            //        var result = await userHelper.AddUserAsync(user, ownerRequest.Password);
            //        var newStatus = await this.ownerRepository.CreateAsync(result);
            //        return Ok(newStatus);

            //    }
        }
        //        [HttpPut("{id}")]
        //        public async Task<IActionResult> PutMaterialType([FromRoute] int id, [FromBody] MAV.Common.Models.OwnerRequest materialType)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return BadRequest(ModelState);
        //            }
        //            if (id != materialType.Id)
        //            {
        //                return BadRequest();
        //            }
        //            var oldMaterialType = await this.materialTypeRepository.GetByIdAsync(id);
        //            if (oldMaterialType == null)
        //            {
        //                return BadRequest("Id not found");
        //            }
        //            oldMaterialType.Name = materialType.Name;
        //            var updateMaterialType = await this.materialTypeRepository.UpdateAsync(oldMaterialType);
        //            return Ok(updateMaterialType);
        //        }

        //        [HttpDelete("{id}")]
        //        public async Task<IActionResult> DeleteMaterialType([FromRoute] int id)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return BadRequest(ModelState);
        //            }
        //            var oldMaterialType = await this.materialTypeRepository.GetByIdAsync(id);
        //            if (oldMaterialType == null)
        //            {
        //                return BadRequest("Id not found");
        //            }
        //            await this.materialTypeRepository.DeleteAsync(oldMaterialType);
        //            return Ok(oldMaterialType);
        //        }
        //    }

        //}
    }
}
