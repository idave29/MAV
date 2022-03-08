namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class MaterialsController : Controller
    {
        private readonly IMaterialRepository materialRepository;

        public MaterialsController(IMaterialRepository materialRepository)
        {
            this.materialRepository = materialRepository;
        }

        [HttpGet]
        public IActionResult GetMaterials()
        {
            return Ok(this.materialRepository.GetMaterials());
            //return Ok(this.materialRepository.GetMaterialWithLoansById(1));
            //return Ok(this.materialRepository.GetMaterialWithLoans());
            //return Ok(this.materialRepository.GetAllMaterialsWithTypeWithStatusAndOwner());
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
                return BadRequest(ModelState);
            }

            var statusNW = new Data.Entities.Status();
            var materialTypeNW = new Data.Entities.MaterialType();
            var ownerNW = new Data.Entities.Owner();
            statusNW.Name = material.Name;
            materialTypeNW.Name = material.Name;
            //ownerNW.User.FirstName = material.Owner;

            var entityStatus = new MAV.Web.Data.Entities.Material
            {
                Name = material.Name,
                Label = material.Label,
                Brand = material.Brand,
                MaterialModel = material.MaterialModel,
                SerialNum = material.SerialNum,
                Status = new Data.Entities.Status()
                {
                    Id = 0,
                    Name = material.Status,
                    Materials = null
                },
                MaterialType = new Data.Entities.MaterialType()
                {
                    Id = 0,
                    Materials = null,
                    Name = material.MaterialType
                },
                Owner = new Data.Entities.Owner()
                {
                    Id=0,
                    Materials= null,
                    User= new Data.Entities.User()
                    {
                        FirstName = material.Owner,
                        LastName = material.Owner,
                        Email = null,
                        PhoneNumber = material.Owner,
                        PasswordHash = "123456"
                    }
                }
                
                //Status = statusNW,
                //MaterialType = materialTypeNW,
                //Owner = ownerNW
            };
            var newMaterial = await this.materialRepository.CreateAsync(entityStatus);
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
            oldMaterial.Name = material.Name;
           // oldMaterial.Owner = material.Owner;
            oldMaterial.Label = material.Label;
            oldMaterial.Brand = material.Brand;
            oldMaterial.MaterialModel = material.MaterialModel;
            oldMaterial.SerialNum = material.SerialNum;
                //oldMaterial.Status = material.Status;
                //oldMaterial.MaterialType = material.MaterialType;
                //oldMaterial.Owner = material.Owner;
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
