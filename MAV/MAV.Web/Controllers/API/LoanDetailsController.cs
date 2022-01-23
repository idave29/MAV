namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LoanDetailsController : Controller
    {
        private readonly ILoanDetailRepository loanDetailRepository;

        public LoanDetailsController(ILoanDetailRepository loanDetailRepository)
        {
            this.loanDetailRepository = loanDetailRepository;
        }

        [HttpGet]
        public IActionResult GetLoanDetail()
        {
            return Ok(this.loanDetailRepository.GetAll());
        }
    }
}
