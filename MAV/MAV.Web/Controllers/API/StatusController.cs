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
            return Ok(this.statusRepository.GetStatus());
            //return Ok(this.statusRepository.GetStatusById(1));
            //return Ok(this.statusRepository.GetStatusByName("Prestado"));
        }

        [HttpPost]
        public async Task<IActionResult> PostStatus([FromBody] MAV.Common.Models.StatusRequest status)
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatus([FromRoute] int id, [FromBody] MAV.Common.Models.StatusRequest status)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(id != status.Id)
            {
                return BadRequest();
            }
            var oldStatus = await this.statusRepository.GetByIdAsync(id);
            if(oldStatus == null)
            {
                return BadRequest("Id not found");
            }
            oldStatus.Name = status.Name;
            var updateStatus = await this.statusRepository.UpdateAsync(oldStatus);
            return Ok(updateStatus);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var oldStatus = await this.statusRepository.GetByIdAsync(id);
            if (oldStatus == null)
            {
                return BadRequest("Id not found");
            }
            await this.statusRepository.DeleteAsync(oldStatus);
            return Ok(oldStatus);
        }
    }
}
