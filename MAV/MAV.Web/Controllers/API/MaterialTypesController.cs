namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MaterialTypesController : Controller
    {
        private readonly IMaterialTypeRepository materialTypeRepository;

        public MaterialTypesController(IMaterialTypeRepository materialTypeRepository)
        {
            this.materialTypeRepository = materialTypeRepository;
        }

        [HttpGet]
        public IActionResult GetMaterialTypes()
        {
            return Ok(this.materialTypeRepository.GetMaterialTypes());
            //return Ok(this.materialTypeRepository.GetMaterialTypeById(1));
            //return Ok(this.materialTypeRepository.GetMaterialTypesByName("Cable"));
        }

        [HttpPost]
        public async Task<IActionResult> PostMaterialType([FromBody] MAV.Common.Models.MaterialTypeRequest materialType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entityMaterialType = new MAV.Web.Data.Entities.MaterialType
            {
                Name = materialType.Name
            };
            var newApplicantType = await this.materialTypeRepository.CreateAsync(entityMaterialType);
            return Ok(newApplicantType);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterialType([FromRoute] int id, [FromBody] MAV.Common.Models.MaterialTypeRequest materialType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != materialType.Id)
            {
                return BadRequest();
            }
            var oldMaterialType = await this.materialTypeRepository.GetByIdAsync(id);
            if (oldMaterialType == null)
            {
                return BadRequest("Id not found");
            }
            oldMaterialType.Name = materialType.Name;
            var updateMaterialType = await this.materialTypeRepository.UpdateAsync(oldMaterialType);
            return Ok(updateMaterialType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterialType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var oldMaterialType = await this.materialTypeRepository.GetByIdAsync(id);
            if (oldMaterialType == null)
            {
                return BadRequest("Id not found");
            }
            await this.materialTypeRepository.DeleteAsync(oldMaterialType);
            return Ok(oldMaterialType);
        }
    }
}
