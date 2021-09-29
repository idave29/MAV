namespace MAV.Web.Controllers
{
    using MAV.Web.Data;
    using MAV.Web.Data.Entities;
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class StatusController : Controller
    {
        private readonly IStatusRepository statusRepository;

        public StatusController(IStatusRepository statusRepository)
        {
            this.statusRepository = statusRepository;
        }

        public IActionResult Index()
        {
            return View(this.statusRepository.GetAll());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await this.statusRepository.GetByIdAsync(id.Value);
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
        public async Task<IActionResult> Create(Status status)
        {
            if (ModelState.IsValid)
            {
                await this.statusRepository.CreateAsync(status);
                return RedirectToAction(nameof(Index));
            }
            return View(status);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await this.statusRepository.GetByIdAsync(id.Value);
            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Status status)
        {
            if (id != status.Id)
            {
                return NotFound();
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
                        return NotFound();
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await this.statusRepository.GetByIdAsync(id.Value);
            if (status == null)
            {
                return NotFound();
            }
            await this.statusRepository.DeleteAsync(status);
            return RedirectToAction(nameof(Index));
        }
    }
}
