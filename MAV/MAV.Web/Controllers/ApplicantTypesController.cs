namespace MAV.Web.Controllers
{
    using MAV.Web.Data;
    using MAV.Web.Data.Entities;
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApplicantTypesController : Controller
    {
        private readonly IApplicantTypeRepository applicantTypeRepository;

        public ApplicantTypesController(IApplicantTypeRepository applicantTypeRepository)
        {
            this.applicantTypeRepository = applicantTypeRepository;
        }

        public IActionResult Index()
        {
            return View(this.applicantTypeRepository.GetAll());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await this.applicantTypeRepository.GetByIdAsync(id.Value);
            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        public IActionResult Create()
        {
            return View();
        }

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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantType = await this.applicantTypeRepository.GetByIdAsync(id.Value);
            if (applicantType == null)
            {
                return NotFound();
            }
            return View(applicantType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ApplicantType applicantType)
        {
            if (id != applicantType.Id)
            {
                return NotFound();
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
                        return NotFound();
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantType = await this.applicantTypeRepository.GetByIdAsync(id.Value);
            if (applicantType == null)
            {
                return NotFound();
            }
            await this.applicantTypeRepository.DeleteAsync(applicantType);
            return RedirectToAction(nameof(Index));
        }
    }
}
