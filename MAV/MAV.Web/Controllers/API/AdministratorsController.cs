namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdministratorsController:Controller
    {
        private readonly IAdministratorRepository administratorRepository;

        public AdministratorsController(IAdministratorRepository administratorRepository)
        {
            this.administratorRepository = administratorRepository;
        }

        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok(this.administratorRepository.GetAdministratorsWithUser());
        }
    }
}
