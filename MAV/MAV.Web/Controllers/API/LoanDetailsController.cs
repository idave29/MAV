namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
    using MAV.Web.Data;
    using MAV.Web.Data.Entities;
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LoanDetailsController : Controller
    {
        private readonly ILoanDetailRepository loanDetailRepository;
        private readonly IMaterialRepository materialRepository;
        private readonly IStatusRepository statusRepository;
        private readonly ILoanRepository loanRepository;
        private readonly IApplicantRepository applicantRepository;
        private readonly DataContext dataContext;

        public LoanDetailsController(ILoanDetailRepository loanDetailRepository, DataContext dataContext, IStatusRepository statusRepository, IMaterialRepository materialRepository, ILoanRepository loanRepository)
        {
            this.loanDetailRepository = loanDetailRepository;
            this.statusRepository = statusRepository;
            this.materialRepository = materialRepository;
            this.loanRepository = loanRepository;
            this.dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult GetLoanDetails()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //return Ok(this.loanDetailRepository.GetLoanDetails()); 
            return Ok(this.loanDetailRepository.GetLoansDetailsWithMaterialAndOwner());
            //return Ok(this.loanDetailRepository.GetLoanDetailsWithMaterialWithoutDateTimeIn()); 
            //return Ok(this.loanDetailRepository.GetLoansDetailsWithMaterialAndOwner());
            //return Ok(this.loanDetailRepository.GetLoanDetailWithMaterialAndOwnerById(1));
            //return Ok(this.loanDetailRepository.GetLoanDetailById(1));
            //DateTime thisDate = new DateTime(2021, 10, 5, 7, 30, 0);
            //return Ok(this.loanDetailRepository.GetLoansDetailsWithMaterialByDateTimeOut(thisDate));
            //return Ok(this.loanDetailRepository.GetLoansDetailsWithMaterialAndOwnerByNameMaterial("HDMI1"));
            //return Ok(this.loanDetailRepository.GetLoanDetailsWithMaterialAndLoan());
        }

        [HttpPost]
        public async Task<IActionResult> PostLoanDetail([FromBody] MAV.Common.Models.LoanDetailsRequest loanDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var entityLoanDetail = new MAV.Web.Data.Entities.LoanDetail
            //{
            //    Observations = loanDetails.Observations,
            //    //Material = loanDetails.Material,
            //    DateTimeIn = loanDetails.DateTimeIn,
            //    DateTimeOut = loanDetails.DateTimeOut
            //};
            //var newLoanDetail = await this.loanDetailRepository.CreateAsync(entityLoanDetail);

            var loan = await this.loanRepository.GetByLoanIdLoanAndApplicantAsync(loanDetails.Loan.Id);
            var status = this.statusRepository.GetStatusByName("Prestado");
            //var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == model.MaterialId);
            var material = await this.materialRepository.GetByIdWithMaterialTypeOwnerStatusAsync(loanDetails.Material.Id);

            material.Status = status;
            await this.materialRepository.UpdateAsync(material);

            var entityLoanDetails = new MAV.Web.Data.Entities.LoanDetail
            {
                Loan = loan,
                DateTimeOut = DateTime.Now,
                DateTimeIn = DateTime.MinValue,
                Material = material,
                Status = status,
                Observations = string.Empty
            };

            if (entityLoanDetails == null)
            {
                return BadRequest("loan not found");
            }

            var newLoanDetail = await this.loanDetailRepository.CreateAsync(entityLoanDetails);

            return Ok(newLoanDetail);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoanDetail([FromRoute] int id, [FromBody] LoanDetailsRequest loanDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != loanDetail.Id)
            {
                return BadRequest();
            }
            var oldLoanDetail = await this.loanDetailRepository.GetByIdLoanDetailAsync(loanDetail.Id);
            if (oldLoanDetail == null)
            {
                return BadRequest("Id not found");
            }

            oldLoanDetail.Observations = loanDetail.Observations;
            oldLoanDetail.DateTimeIn = DateTime.Now;

            //var status = this.statusRepository.GetStatusByName("Regresado");
            var status = await this.statusRepository.GetByIdStatusAsync(3);
            oldLoanDetail.Status = status;

            if (oldLoanDetail.Status == null)
            {
                return BadRequest("status not found");
            }

            var debtor = false;

            foreach (Loan loanApp in oldLoanDetail.Loan.Applicant.Loans)
            {
                foreach (LoanDetail loanDetails in loanApp.LoanDetails)
                {
                    if (loanDetails.Status.Id == 2)
                    {
                        debtor = true;
                    }
                }
            }

            status = await this.statusRepository.GetByIdStatusAsync(2);
            oldLoanDetail.Material.Status = status;

            oldLoanDetail.Loan.Applicant.Debtor = debtor;

            await this.applicantRepository.UpdateAsync(oldLoanDetail.Loan.Applicant);
            await this.materialRepository.UpdateAsync(oldLoanDetail.Material);
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
