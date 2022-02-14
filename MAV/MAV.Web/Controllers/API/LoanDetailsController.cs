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
    public class LoanDetailsController : Controller
    {
        private readonly ILoanDetailRepository loanDetailRepository;
        private readonly DataContext dataContext;

        public LoanDetailsController(ILoanDetailRepository loanDetailRepository, DataContext dataContext)
        {
            this.loanDetailRepository = loanDetailRepository;
            this.dataContext = dataContext;
        }

        //[HttpGet]
        public IActionResult GetLoanDetail()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailApplicant = new EmailRequest { Email = "natalia.xambrano@gmail.com" };
            return Ok(this.loanDetailRepository.GetLoanDetailsWithEmail(emailApplicant));
        }

        [HttpPost]
        public async Task<IActionResult> PostLoanDetail([FromBody] MAV.Common.Models.LoanDetails loanDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entityLoanDetail = new MAV.Web.Data.Entities.LoanDetail
            {
                Observations = loanDetails.Observations,
                //Material = loanDetails.Material,
                DateTimeIn = loanDetails.DateTimeIn,
                DateTimeOut = loanDetails.DateTimeOut
            };
            var newLoanDetail = await this.loanDetailRepository.CreateAsync(entityLoanDetail);
            return Ok(newLoanDetail);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoanDetail([FromRoute] int id, [FromBody] MAV.Common.Models.LoanDetails loanDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != loanDetail.Id)
            {
                return BadRequest();
            }
            var oldLoanDetail = await this.loanDetailRepository.GetByIdAsync(id);
            if (oldLoanDetail == null)
            {
                return BadRequest("Id not found");
            }
            oldLoanDetail.Observations = loanDetail.Observations;
            //oldLoanDetail.Material = loanDetail.Material;
            oldLoanDetail.DateTimeIn = loanDetail.DateTimeIn;
            oldLoanDetail.DateTimeOut = loanDetail.DateTimeOut;
            var updateLoanDetail = await this.loanDetailRepository.UpdateAsync(oldLoanDetail);
            return Ok(updateLoanDetail);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoanDetail([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var oldLoanDetail = await this.loanDetailRepository.GetByIdAsync(id);
            if (oldLoanDetail == null)
            {
                return BadRequest("Id not found");
            }
            await this.loanDetailRepository.DeleteAsync(oldLoanDetail);
            return Ok(oldLoanDetail);
        }
    }
}
