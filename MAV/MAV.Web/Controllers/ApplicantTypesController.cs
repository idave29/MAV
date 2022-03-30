namespace MAV.Web.Controllers
{
    using MAV.Web.Data;
    using MAV.Web.Data.Entities;
    using MAV.Web.Data.Repositories;
    using MAV.Web.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApplicantTypesController : Controller
    {
        private readonly IApplicantTypeRepository applicantTypeRepository;
        private readonly DataContext _context;

        public ApplicantTypesController(DataContext context, IApplicantTypeRepository applicantTypeRepository)
        {
            _context = context;
            this.applicantTypeRepository = applicantTypeRepository;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            return View(this.applicantTypeRepository.GetAll());
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ApplicantTypeNotFound");
            }

            var status = await this.applicantTypeRepository.GetByIdAsync(id.Value);
            if (status == null)
            {
                return new NotFoundViewResult("ApplicantTypeNotFound");
            }

            return View(status);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicantType applicantType)
        {
            if (ModelState.IsValid)
            {
                await this.applicantTypeRepository.CreateAsync(applicantType);
                return RedirectToAction(nameof(Index));
            }
            return View(applicantType);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ApplicantTypeNotFound");
            }

            var applicantType = await this.applicantTypeRepository.GetByIdAsync(id.Value);
            if (applicantType == null)
            {
                return new NotFoundViewResult("ApplicantTypeNotFound");
            }
            return View(applicantType);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ApplicantType applicantType)
        {
            if (id != applicantType.Id)
            {
                return new NotFoundViewResult("ApplicantTypeNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.applicantTypeRepository.UpdateAsync(applicantType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.applicantTypeRepository.ExistAsync(applicantType.Id))
                    {
                        return new NotFoundViewResult("ApplicantTypeNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicantType);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ApplicantTypeNotFound");
            }

            var applicantType = await this.applicantTypeRepository.GetByIdAplicantTypeAsync(id.Value);
            if (applicantType == null)
            {
                return new NotFoundViewResult("ApplicantTypeNotFound");
            }

            if (applicantType.Applicants.Count != 0 && applicantType.Applicants != null)
            {
                ModelState.AddModelError(string.Empty, "This type is used in one or more applicant, delete them first before deleting this.");
                return RedirectToAction("Index", "ApplicantTypes");
            }

            return View(applicantType);
        }

        [Authorize(Roles = "Administrador")]
        // POST: ApplicantTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicantType = await _context.ApplicantTypes.FindAsync(id);
            _context.ApplicantTypes.Remove(applicantType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        private bool ApplicantTypeExists(int id)
        {
            return _context.ApplicantTypes.Any(e => e.Id == id);
        }

    }
}
