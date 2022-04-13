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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class LoansController : ControllerBase
    {
        private readonly ILoanRepository loanRepository;
        private readonly DataContext dataContext;

        private readonly IInternRepository internRepository;
        private readonly IApplicantRepository applicantRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IMaterialRepository materialRepository;
        private readonly ILoanDetailRepository loanDetailRepository;
        public LoansController(ILoanRepository loanRepository, DataContext dataContext, IInternRepository internRepository, IApplicantRepository applicantRepository, IStatusRepository statusRepository, IMaterialRepository materialRepository, ILoanDetailRepository loanDetailRepository)
        {
            this.loanRepository = loanRepository;
            this.dataContext = dataContext;
            this.internRepository = internRepository;
            this.applicantRepository = applicantRepository;
            this.statusRepository = statusRepository;
            this.materialRepository = materialRepository;
            this.loanDetailRepository = loanDetailRepository;

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
        public async Task<IActionResult> PostLoan([FromBody] LoanRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicant = this.applicantRepository.GetApplicantByName(model.Applicant);
            if (applicant == null)
            {
                return BadRequest("Solicitante not found");
            }
            //var entityLoan = new MAV.Web.Data.Entities.Loan
            //{
            //    Intern = intern,
            //    Applicant = applicant
            //};

            //if (entityLoan == null)
            //{
            //    return BadRequest("loan not found");
            //}
            //var newLoan = await this.loanRepository.CreateAsync(entityLoan);

            //if (ModelState.IsValid)
            //{

            //var applicant = await _context.Applicants.FirstOrDefaultAsync(m => m.Id == model.ApplicantId);
            //var intern = await _context.Interns.FirstOrDefaultAsync();
            var intern = await this.internRepository.GetByIdInternWithLoansLoansDetailsAsync(1);
            foreach (Intern internObj in this.internRepository.GetInternsWithUser())
                //_context.Interns.Include(s => s.User))
            {
                if (internObj.User.UserName == this.User.Identity.Name)
                {
                    intern = internObj;
                }
            }
            if (intern == null)
            {
                return BadRequest("Becario not found");
            }
            
            var loan = new MAV.Web.Data.Entities.Loan { Applicant = applicant, Intern = intern };
            if (loan == null)
            {
                return BadRequest("loan not found");
            }

            //var status = _context.Statuses.FirstOrDefault(m => m.Id == 2);
            var status = this.statusRepository.GetStatusByName("Prestado");
            //var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == model.MaterialId);
            var idm = Convert.ToInt32(model.LoanDetails.Select(l => l.Material.Id).FirstOrDefault().ToString());

            //List<int> mat = model.LoanDetails.Select(l => l.Material.Id).ToList();
            //mat.FirstOrDefault().ToString;

            var material = await this.materialRepository.GetByIdWithMaterialTypeOwnerStatusAsync(idm);



            var entityLoanDetails = new MAV.Web.Data.Entities.LoanDetail
            {
                Loan = loan,
                DateTimeOut = DateTime.Now,
                DateTimeIn = DateTime.MinValue,
                Material = material,
                Status = material.Status,
                Observations = string.Empty
            };

            if (entityLoanDetails == null)
            {
                return BadRequest("loan not found");
            }

            await this.loanDetailRepository.CreateAsync(entityLoanDetails);
            //_context.LoanDetails.Add(new LoanDetail { Loan = loan, DateTimeOut = DateTime.Now, DateTimeIn = DateTime.MinValue, Material = material, Status = status, Observations = string.Empty });

            material.Status = status;
            applicant.Debtor = true;

            await this.materialRepository.UpdateAsync(material);
            await this.applicantRepository.UpdateAsync(applicant);
            var newLoan = await this.loanRepository.CreateAsync(loan);

            return Ok(newLoan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan([FromRoute] int id, [FromBody] LoanRequest loan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != loan.Id)
            {
                return BadRequest();
            }

            var oldLoan = await this.loanRepository.GetByIdAsync(id);
            if (oldLoan == null)
            {
                return BadRequest("loan not found");
            }

            var intern = this.internRepository.GetInternByName(loan.Intern);
            if (intern == null)
            {
                return BadRequest("intern not found");
            }
            var applicant = this.applicantRepository.GetApplicantByName(loan.Applicant);
            if (applicant == null)
            {
                return BadRequest("applicant not found");
            }

            oldLoan.Intern = intern;
            oldLoan.Applicant = applicant;

            var updateLoan = await this.loanRepository.UpdateAsync(oldLoan);

            return Ok(updateLoan);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var oldLoan = await this.loanRepository.GetByIdAsync(id);

            if (oldLoan == null)
            {
                return BadRequest("Id not found");
            }
            ;
            await this.loanRepository.DeleteAsync(oldLoan);
            return Ok(oldLoan);

        }
    }
}