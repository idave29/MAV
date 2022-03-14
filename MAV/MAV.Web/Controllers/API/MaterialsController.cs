namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
    using MAV.Web.Data.Repositories;
    using MAV.Web.Helpers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class MaterialsController : Controller
    {
        private readonly IMaterialRepository materialRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IMaterialTypeRepository materialTypeRepository;
        private readonly IOwnerRepository ownerRepository;
        private readonly IUserHelper userHelper;

        public MaterialsController(IMaterialRepository materialRepository, IStatusRepository statusRepository, IMaterialTypeRepository materialTypeRepository,
            IOwnerRepository ownerRepository, IUserHelper userHelper)
        {
            this.materialRepository = materialRepository;
            this.statusRepository = statusRepository;
            this.materialTypeRepository = materialTypeRepository;
            this.ownerRepository = ownerRepository;
            this.userHelper = userHelper;
        }


        [HttpGet]
        public IActionResult GetMaterials()
        {
           // return Ok(this.materialRepository.GetMaterials());
            //return Ok(this.materialRepository.GetMaterialWithLoansById(1));
            //return Ok(this.materialRepository.GetMaterialWithLoans());
            return Ok(this.materialRepository.GetAllMaterialsWithTypeWithStatusAndOwner());
            //return Ok(this.materialRepository.GetMaterialWithTypeWithStatusAndOwnerById(1));
            //return Ok(this.materialRepository.GetMaterialWithTypeAndStatusBySerialNum("6817654"));
            //return Ok(this.materialRepository.GetMaterialBySerialNum("897654"));
            //return Ok(this.materialRepository.GetMaterialsWithTypeWithStatusAndOwnerByBrand("Sony"));
            //return Ok(this.materialRepository.GetMaterialsWithTypeWithStatusAndOwnerByStatus("Disponible"));
            //return Ok(this.materialRepository.GetMaterialsWithTypeWithStatusAndOwnerByType("Cable"));
            //return Ok(this.materialRepository.GetMaterialsWithTypeWithStatusAndOwnerByName("VGA"));
            //return Ok(this.materialRepository.GetMaterialsWithTypeWithStatusAndOwnerByLabel("MAV01"));
        }

        [HttpPost]
        public async Task<IActionResult> PostMaterial([FromBody] MAV.Common.Models.MaterialRequest material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ModelState mal");
            }

            var status = this.statusRepository.GetStatusByName(material.Status);
            if (status == null)
            {
                return BadRequest("status not found");
            }
            var materialType = this.materialTypeRepository.GetMaterialTypesByName(material.MaterialType);
            if (materialType == null)
            {
                return BadRequest("materialtype not found");
            }
            var owner = this.ownerRepository.GetGoodOwnerWithEmail(material.Owner);          
            if (owner == null)
            {
                return BadRequest("owner not found");
            }
            //else
            //{
            //    return BadRequest("Owner bien");
            //}


            var entityMaterial = new MAV.Web.Data.Entities.Material
            {
                Name = material.Name,
                Owner = owner,
                Status = status,
                MaterialType = materialType,
                Brand = material.Brand,
                Label = material.Label,
                MaterialModel = material.MaterialModel,
                SerialNum = material.SerialNum
            };
            if (entityMaterial == null)
            {
                return BadRequest("entityMaterial not found");
            }
            var newMaterial = await this.materialRepository.CreateAsync(entityMaterial);
            return Ok(newMaterial);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterial([FromRoute] int id, [FromBody] MAV.Common.Models.MaterialRequest material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != material.Id)
            {
                return BadRequest();
            }
            var oldMaterial = await this.materialRepository.GetByIdAsync(id);
            if (oldMaterial == null)
            {
                return BadRequest("Id not found");
            }
            var status = this.statusRepository.GetStatusByName(material.Status);
            if (status == null)
            {
                return BadRequest("status not found");
            }
            var materialType = this.materialTypeRepository.GetMaterialTypesByName(material.MaterialType);
            if (materialType == null)
            {
                return BadRequest("materialtype not found");
            }
            var owner = this.ownerRepository.GetGoodOwnerWithEmail(material.Owner);
            if (owner == null)
            {
                return BadRequest("owner not found");
            }
            oldMaterial.Name = material.Name;
            oldMaterial.Owner = owner;
            oldMaterial.Label = material.Label;
            oldMaterial.Brand = material.Brand;
            oldMaterial.MaterialModel = material.MaterialModel;
            oldMaterial.SerialNum = material.SerialNum;
            oldMaterial.Status = status;
            oldMaterial.MaterialType = materialType;
            var updateMaterial = await this.materialRepository.UpdateAsync(oldMaterial);
            return Ok(updateMaterial);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var oldMaterial = await this.materialRepository.GetByIdAsync(id);
            if (oldMaterial == null)
            {
                return BadRequest("Id not found");
            }
            await this.materialRepository.DeleteAsync(oldMaterial);
            return Ok(oldMaterial);
        }
    }
}
