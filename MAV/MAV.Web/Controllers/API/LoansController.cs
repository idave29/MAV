namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Mvc;
    [Route("api/[Controller]")]

    public class LoansController : Controller
    {
        private readonly ILoanRepository loanRepository;

        public LoansController(ILoanRepository loanRepository)
        {
            this.loanRepository = loanRepository;
        }

        [HttpGet]
        public IActionResult GetLoan()
        {
            return Ok(this.loanRepository.GetAll());
        }
    }
}