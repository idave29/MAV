namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Mvc;
    [Route("api/[Controller]")]
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
            return Ok(this.administratorRepository.GetAll());
        }
    }
}
