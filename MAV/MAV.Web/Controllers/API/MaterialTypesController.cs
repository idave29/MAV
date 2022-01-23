namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            return Ok(this.materialTypeRepository.GetAll());
        }
    }
}
