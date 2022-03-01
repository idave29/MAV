namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
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
        public IActionResult GetAdministrators()
        {
            //return Ok(this.administratorRepository.GetAdministratorsWithUserByName("Miguel"));
            //var emailApplicant = new EmailRequest { Email = "miguel.ochoa@gmail.com" };
            //return Ok(this.administratorRepository.GetAdministratorWithUserByEmail(emailApplicant));
            //return Ok(this.administratorRepository.GetAdministratorWithUserById(1));
            //return Ok(this.administratorRepository.GetAdministratorsWithUser());
            return Ok(this.administratorRepository.GetAdministrators());
        }

    }
}
