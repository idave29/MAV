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

    public class StatusController : Controller
    {
        private readonly IStatusRepository statusRepository;
        private readonly DataContext _context;

        public StatusController(DataContext context, IStatusRepository statusRepository)
        {
            _context = context;
            this.statusRepository = statusRepository;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            return View(this.statusRepository.GetAll());
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("StatusNotFound");
            }

            var status = await this.statusRepository.GetByIdAsync(id.Value);
            if (status == null)
            {
                return new NotFoundViewResult("StatusNotFound");
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
        public async Task<IActionResult> Create(Status status)
        {
            if (ModelState.IsValid)
            {
                await this.statusRepository.CreateAsync(status);
                return RedirectToAction(nameof(Index));
            }
            return View(status);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("StatusNotFound");
            }

            var status = await this.statusRepository.GetByIdAsync(id.Value);
            if (status == null)
            {
                return new NotFoundViewResult("StatusNotFound");
            }
            return View(status);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Status status)
        {
            if (id != status.Id)
            {
                return new NotFoundViewResult("StatusNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.statusRepository.UpdateAsync(status);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.statusRepository.ExistAsync(status.Id))
                    {
                        return new NotFoundViewResult("StatusNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(status);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("StatusNotFound");
            }

            var status = await this.statusRepository.GetByIdStatusAsync(id.Value);
            if (status == null)
            {
                return new NotFoundViewResult("StatusNotFound");
            }

            if (status.Materials.Count != 0)
            {
                ModelState.AddModelError(string.Empty, "This type is used in one or more applicant, delete them first before deleting this.");
                return RedirectToAction("Index", "Status");
            }

            if (status.LoanDetails.Count != 0)
            {
                ModelState.AddModelError(string.Empty, "This type is used in one or more applicant, delete them first before deleting this.");
                return RedirectToAction("Index", "Status");
            }

            return View(status);
        }

        [Authorize(Roles = "Administrador")]
        // POST: ApplicantTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var status = await _context.Statuses.FindAsync(id);
            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        private bool StatusesExists(int id)
        {
            return _context.Statuses.Any(e => e.Id == id);
        }
    }
}
