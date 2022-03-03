namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
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
            //return Ok(this.materialRepository.GetMaterials());
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
    }
}
