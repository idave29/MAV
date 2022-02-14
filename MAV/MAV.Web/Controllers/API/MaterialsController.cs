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
        public IActionResult GetMaterial()
        {
            return Ok(this.materialRepository.GetMaterials());
        }
    }
}
