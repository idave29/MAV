namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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
        [HttpPost]
        public async Task<IActionResult> PostLoan([FromBody] MAV.Common.Models.Loan loan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entityLoan = new MAV.Web.Data.Entities.Loan
            {
                //Intern = loan.Intern,
                //LoanDetails = loan.LoanDetails
            };
            var newLoan = await this.loanRepository.CreateAsync(entityLoan);
            return Ok(newLoan);
        }
    }
}