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
            var a = this.dataContext.Applicants
    .Include(a => a.User)
    .Include(a => a.Loans)
    .ThenInclude(l => l.LoanDetails)
    .ThenInclude(ld => ld.Material)
    .ThenInclude(m => m.Status)
    .Include(a => a.Loans)
    .ThenInclude(l => l.LoanDetails)
    .ThenInclude(ld => ld.Material)
    .ThenInclude(m => m.MaterialType)
    .Include(a => a.Loans)
    .ThenInclude(l => l.Intern)
    .ThenInclude(a => a.User)
    .Include(a => a.ApplicantType)
    .FirstOrDefault(a => a.User.Email.ToLower() == emailApplicant.Email);

            if (a == null)
            {
                return NotFound();
            }
            var x = new ApplicantRequest
            {
                Id = a.Id,
                FirstName = a.User.FirstName, 
                LastName = a.User.LastName,
                Email = a.User.Email,
                PhoneNumber = a.User.PhoneNumber,
                ApplicantType = a.ApplicantType.Name,
                Loans = a.Loans?.Select(l => new LoanRequest { 
                    Id = l.Id,
                    Intern = new InternRequest
                    {
                        Id = l.Intern.Id,
                        Email = l.Intern.User.Email,
                        FirstName = l.Intern.User.FirstName,
                        LastName= l.Intern.User.LastName,
                        PhoneNumber = l.Intern.User.PhoneNumber,
                    },
                    LoanDetails = l.LoanDetails?.Select(ld => new LoanDetailsRequest
                    {
                        Id = ld.Id,
                        DateTimeIn = ld.DateTimeIn,
                        DateTimeOut = ld.DateTimeOut,
                        Observations = ld.Observations,
                        Material = new MaterialRequest
                        {
                            Id = ld.Material.Id,
                            Brand = ld.Material.Brand,
                            Label = ld.Material.Label,
                            MaterialModel = ld.Material.MaterialModel,
                            MaterialType = ld.Material.MaterialType.Name,
                            Name = ld.Material.Name,
                            SerialNum = ld.Material.SerialNum,
                            Status = ld.Material.Status.Name
                        }
                    }).Where(ld => ld.Observations != null).ToList()
                }).ToList()
            };
            return Ok(x);
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