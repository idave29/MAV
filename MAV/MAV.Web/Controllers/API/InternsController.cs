namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InternsController:Controller
    {
        private readonly IInternRepository internRepository;

        public InternsController(IInternRepository internRepository)
        {
            this.internRepository = internRepository;
        }

        [HttpGet]
        public IActionResult GetInterns()
        {
            return Ok(this.internRepository.GetInterns());
            //return Ok(this.internRepository.GetInternsWithUser());
            //return Ok(this.internRepository.GetInternsWithLoansLoanDetailsWithMaterialAndOwner());
            //var emailIntern = new EmailRequest { Email = "deivi.hr@gmail.com" };
            //return Ok(this.internRepository.GetInternWithLoansLoanDetailsWithMaterialAndOwnerByEmail(emailIntern));
            //return Ok(this.internRepository.GetInternWithLoansByEmail(emailIntern));
            //return Ok(this.internRepository.GetInternsWithLoansLoanDetailsWithMaterialAndOwnerByName("Arturo"));
            //return Ok(this.internRepository.GetInternsWithLoansLoanDetailsWithMaterialAndOwnerById(1));
        }
    }
}
