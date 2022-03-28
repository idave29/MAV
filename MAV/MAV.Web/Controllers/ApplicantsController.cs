using MAV.Web.Data;
using MAV.Web.Data.Entities;
using MAV.Web.Data.Repositories;
using MAV.Web.Helpers;
using MAV.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MAV.Web.Controllers
{
    public class ApplicantsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper combosHelper;
        private readonly IUserHelper userHelper;
        private readonly IApplicantRepository applicantRepository;
        private readonly ILoanRepository loanRepository;
        private readonly ILoanDetailRepository loanDetailRepository;

        public ApplicantsController(DataContext context, IApplicantRepository applicantRepository,
            ICombosHelper combosHelper, IUserHelper userHelper, ILoanRepository loanRepository, ILoanDetailRepository loanDetailRepository)
        {
            _context = context;
            this.applicantRepository = applicantRepository;
            this.combosHelper = combosHelper;
            this.userHelper = userHelper;
            this.loanDetailRepository = loanDetailRepository;
            this.loanRepository = loanRepository;
        }

        [Authorize(Roles = "Administrador, Responsable")]
        // GET: Applicants
        public IActionResult Index()
        {
            return View(this.applicantRepository.GetApplicantsWithUser());
        }

        [Authorize(Roles = "Administrador, Responsable")]
        // GET: Applicants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ApplicantNotFound");
            }

            var applicant = await this.applicantRepository.GetByIdWithUserAsync(id.Value);
            if (applicant == null)
            {
                return new NotFoundViewResult("ApplicantNotFound");
            }

            return View(applicant);
        }

        // GET: Applicants/Create
        public IActionResult Create()
        {
            var model = new ApplicantViewModel
            {
                Users = combosHelper.GetComboUsers(),
                Types = combosHelper.GetComboApplicantTypes()
            };

            //No carga la seleccion de combo box
            return View(model);
        }

        [Authorize(Roles = "Administrador, Responsable")]
        // POST: Applicants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicantViewModel model)
        {
            if (model.UserUserName != "(Debe de escoger un usuario)" && model.TypeId != 0)
            {
                var user = await userHelper.GetUserByEmailAsync(model.UserUserName);

                if (user == null)
                {
                    return new NotFoundViewResult("ApplicantNotFound");
                }

                foreach (Applicant applTemp in _context.Applicants.Include(c => c.User))
                {
                    if (applTemp.User == user)
                    {
                        ModelState.AddModelError(string.Empty, "Solicitante ya existe");
                        return View(model);
                    }
                }

                var type = await _context.ApplicantTypes.FirstOrDefaultAsync(m => m.Id == model.TypeId);
                var applicant = new Applicant { User = user, ApplicantType = type, Debtor = model.Debtor };


                await userHelper.AddUserToRoleAsync(user, "Solicitante");

                _context.Add(applicant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Applicants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant == null)
            {
                return NotFound();
            }
            return View(applicant);
        }

        // POST: Applicants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Applicant applicant)
        {
            if (id != applicant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicantExists(applicant.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicant);
        }

        // GET: Applicants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ApplicantNotFound");
            }

            var applicant = await this.applicantRepository.GetByIdWithUserAsync(id.Value);
            if (applicant == null)
            {
                return new NotFoundViewResult("ApplicantNotFound");
            }

            return View(applicant);
        }

        // POST: Applicants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicant = await this.applicantRepository.GetByIdWithUserAsync(id);
            //var user = await this.userHelper.GetUserByIdAsync(applicant.User.Id);
            if (applicant.User == null)
            {
                return new NotFoundViewResult("ApplicantNotFound");
            }

            var loanDetailUser = await this.loanDetailRepository.GetByIdAppOrInternLoanDetailsAsync(applicant.User.Id);
            if (loanDetailUser != null)
                await this.loanDetailRepository.DeleteAsync(loanDetailUser);

            //Agregar material para cambiarle el estatus y el loan detail se convierta en null

            var loanUser = await this.loanRepository.GetByIdAppOrInternLoansAsync(applicant.User.Id);
            if (loanUser != null)
                await this.loanRepository.DeleteAsync(loanUser);

            await userHelper.RemoveUserFromRoleAsync(applicant.User, "Solicitante");
            await this.applicantRepository.DeleteAsync(applicant);
            //await this.userHelper.DeleteUserAsync(user);
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicantExists(int id)
        {
            return _context.Applicants.Any(e => e.Id == id);
        }
    }
}
