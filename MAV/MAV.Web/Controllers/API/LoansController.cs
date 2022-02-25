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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(this.loanRepository.GetLoans());
            //return Ok(this.loanRepository.GetLoansWithInternsAndLoanDetails());
            //return Ok(this.loanRepository.GetLoansWithLoanDetailsAndMaterial());
            //return Ok(this.loanRepository.GetLoanWithLoanDetailsById(1));
            //return Ok(this.loanRepository.GetLoanWithLoanDetailsAndMaterialById(1));
            //return Ok(this.loanRepository.GetLoansWithLoanDetailsWithMaterialAndOwnerByNameMaterial("VGA"));

        }

        [HttpPost]
        public async Task<IActionResult> PostLoan([FromBody] MAV.Common.Models.LoanRequest loan)
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