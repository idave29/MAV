using MAV.Web.Data;
using MAV.Web.Data.Entities;
using MAV.Web.Data.Repositories;
using MAV.Web.Helpers;
using MAV.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MAV.Web.Controllers
{
    public class LoanDetailsController : Controller
    {
        private readonly DataContext _context;

        private readonly ILoanDetailRepository loanDetailRepository;
        private readonly ICombosHelper combosHelper;
        private readonly IImageHelper imageHelper;
        private readonly IUserHelper userHelper;

        public LoanDetailsController(ILoanDetailRepository loanDetailRepository, DataContext context,
            ICombosHelper combosHelper,
            IImageHelper imageHelper,
            IUserHelper userHelper)
        {
            this.loanDetailRepository = loanDetailRepository;
            _context = context;
            this.combosHelper = combosHelper;
            this.imageHelper = imageHelper;
            this.userHelper = userHelper;
        }

        // GET: LoanDetails
        public IActionResult Index()
        {
            return View(this.loanDetailRepository.GetLoanDetailsWithMaterialAndLoan());
        }

        // GET: LoanDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("LoanDetailNotFound");
            }

            var loandetail = await _context.LoanDetails
                .Include(s => s.Status)
                .Include(s => s.Material)
                .Include(s => s.Loan)
                .ThenInclude(c => c.Applicant)
                .ThenInclude(v => v.User)
                .Include(s => s.Loan)
                .ThenInclude(c => c.Intern)
                .ThenInclude(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loandetail == null)
            {
                return new NotFoundViewResult("LoanDetailNotFound");
            }

            if (this.User.Identity.Name != loandetail.Loan.Intern.User.UserName && !this.User.IsInRole("Responsable") && !this.User.IsInRole("Administrador"))
                return new NotFoundViewResult("LoanDetailNotFound");

            return View(loandetail);
        }

        // GET: LoanDetails/Create
        public IActionResult Create(int? id)
        {
            var model = new LoanDetailViewModel
            {
                StatusId = 2,
                Statuses = combosHelper.GetComboStatuses(),
                Materials = combosHelper.GetComboMaterials(),
                Loan = _context.Loans.Include(s => s.Intern).ThenInclude(c => c.User).FirstOrDefault(m => m.Id == id),
            };

            //if (this.User.Identity.Name != model.Loan.Intern.User.UserName)
            //    return new NotFoundViewResult("LoanDetailNotFound");

            model.LoanID = model.Loan.Id;

            return View(model);
        }

        // POST: LoanDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoanDetailViewModel model)
        {
            var status = _context.Statuses.FirstOrDefault(m => m.Id == 2);
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == model.MaterialId);
            var loan = await _context.Loans.FirstOrDefaultAsync(m => m.Id == model.LoanID);

            material.Status = status;

            _context.Materials.Update(material);

            var ld = new LoanDetail { Loan = loan, DateTimeOut = DateTime.Now, DateTimeIn = DateTime.MinValue, Material = material, Status = status, Observations = string.Empty };


            _context.LoanDetails.Add(ld);
            await _context.SaveChangesAsync();
            return RedirectToAction(String.Format("Details/{0}", ld.Loan.Id), "Loans");
        }

        // GET: LoanDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("LoanDetailNotFound");
            }

            var loanDetail = await _context.LoanDetails
                .Include(s => s.Status)
                .Include(s => s.Material)
                .Include(s => s.Loan)
                .ThenInclude(c => c.Intern)
                .ThenInclude(v => v.User)
                .Include(s => s.Loan)
                .ThenInclude(c => c.Applicant)
                .ThenInclude(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loanDetail == null)
            {
                return new NotFoundViewResult("LoanDetailNotFound");
            }

            var model = new LoanDetailViewModel
            {
                Id = loanDetail.Id,
                Observations = loanDetail.Observations,
                DateTimeIn = loanDetail.DateTimeIn,
                DateTimeOut = loanDetail.DateTimeOut,
                Material = loanDetail.Material,
                Status = loanDetail.Status,
                Loan = loanDetail.Loan,
                Statuses = combosHelper.GetComboStatuses(),
                Materials = combosHelper.GetComboMaterials()
            };

            if (this.User.Identity.Name != loanDetail.Loan.Intern.User.UserName && !this.User.IsInRole("Owner") && !this.User.IsInRole("Administrator"))
                return new NotFoundViewResult("LoanDetailNotFound");

            return View(model);
        }

        // POST: LoanDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LoanDetailViewModel model)
        {
            var ld = await _context.LoanDetails
                .Include(s => s.Loan)
                .ThenInclude(c => c.Applicant)
                .ThenInclude(x => x.Loans)
                .ThenInclude(y => y.LoanDetails)
                .ThenInclude(z => z.Status)
                .Include(s => s.Material)
                .ThenInclude(c => c.Status)
                .FirstOrDefaultAsync(m => m.Id == model.Id);

            if (ld == null)
            {
                return new NotFoundViewResult("LoanDetailNotFound");
            }

            ld.Observations = model.Observations;
            ld.DateTimeIn = DateTime.Now;

            var status = await _context.Statuses.FirstOrDefaultAsync(m => m.Id == 3);
            ld.Status = status;

            var debtor = false;

            foreach (Loan loanApp in ld.Loan.Applicant.Loans)
            {
                foreach (LoanDetail loanDetails in loanApp.LoanDetails)
                {
                    if (loanDetails.Status.Id == 2)
                    {
                        debtor = true;
                    }
                }
            }

            status = await _context.Statuses.FirstOrDefaultAsync(m => m.Id == 1);
            ld.Material.Status = status;

            ld.Loan.Applicant.Debtor = debtor;

            _context.Applicants.Update(ld.Loan.Applicant);
            _context.Materials.Update(ld.Material);
            _context.Update(ld);
            await _context.SaveChangesAsync();
            return RedirectToAction(String.Format("Details/{0}", ld.Loan.Id), "Loans");
        }

        // GET: LoanDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("LoanDetailNotFound");
            }

            var loandetail = await _context.LoanDetails
                .Include(s => s.Status)
                .Include(s => s.Material)
                .Include(s => s.Loan)
                .ThenInclude(c => c.Applicant)
                .ThenInclude(v => v.User)
                .Include(s => s.Loan)
                .ThenInclude(c => c.Intern)
                .ThenInclude(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loandetail == null)
            {
                return new NotFoundViewResult("LoanDetailNotFound");
            }

            return View(loandetail);
        }

        // POST: LoanDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanDetail = await _context.LoanDetails.Include(s => s.Loan).FirstOrDefaultAsync(m => m.Id == id);
            _context.LoanDetails.Remove(loanDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(String.Format("Details/{0}", loanDetail.Loan.Id), "Loans");
        }

        //private bool LoanDetailExists(int id)
        //{
        //    return _context.LoanDetails.Any(e => e.Id == id);
        //}

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }
    }
}
