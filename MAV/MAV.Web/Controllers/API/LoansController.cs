namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
    using MAV.Web.Data;
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class LoansController : ControllerBase
    {
        private readonly ILoanRepository loanRepository;
        private readonly DataContext dataContext;

        public LoansController(ILoanRepository loanRepository, DataContext dataContext)
        {
            this.loanRepository = loanRepository;
            this.dataContext = dataContext;
        }

        //[HttpPost]
        //[Route("GetLoanByEmail")]
        public IActionResult GetLoansController()
        {
           var emailApplicant = new EmailRequest {Email = "natalia.xambrano@gmail.com" };
            return Ok(this.loanRepository.GetLoans(emailApplicant));
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