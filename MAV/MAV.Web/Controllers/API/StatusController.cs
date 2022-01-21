namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme) ]
    public class StatusController : Controller
    {
        private readonly IStatusRepository statusRepository;

        public StatusController(IStatusRepository statusRepository)
        {
            this.statusRepository = statusRepository;
        }

        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok(this.statusRepository.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> PostStatus([FromBody] MAV.Common.Models.Status status)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entityStatus = new MAV.Web.Data.Entities.Status
            {
                Name = status.Name
            }; 
            var newStatus = await this.statusRepository.CreateAsync(entityStatus);
            return Ok(newStatus);
        }
    }
}
