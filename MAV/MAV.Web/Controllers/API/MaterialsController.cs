namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Mvc;
    [Route("api/[Controller]")]

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
            return Ok(this.materialRepository.GetAll());
        }
    }
}
