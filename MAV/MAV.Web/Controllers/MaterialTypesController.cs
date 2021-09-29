namespace MAV.Web.Controllers
{
    using MAV.Web.Data.Entities;
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class MaterialTypesController : Controller
    {
        private readonly IMaterialTypeRepository materialTypeRepository;

        public MaterialTypesController(IMaterialTypeRepository materialTypeRepository)
        {
            this.materialTypeRepository = materialTypeRepository;
        }

        public IActionResult Index()
        {
            return View(this.materialTypeRepository.GetAll());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDay = await this.materialTypeRepository.GetByIdAsync(id.Value);
            if (weekDay == null)
            {
                return NotFound();
            }

            return View(weekDay);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialType materialType)
        {
            if (ModelState.IsValid)
            {
                await this.materialTypeRepository.CreateAsync(materialType);
                return RedirectToAction(nameof(Index));
            }
            return View(materialType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDay = await this.materialTypeRepository.GetByIdAsync(id.Value);
            if (weekDay == null)
            {
                return NotFound();
            }
            return View(weekDay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialType materialType)
        {
            if (id != materialType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.materialTypeRepository.UpdateAsync(materialType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.materialTypeRepository.ExistAsync(materialType.Id))
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
            return View(materialType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDay = await this.materialTypeRepository.GetByIdAsync(id.Value);
            if (weekDay == null)
            {
                return NotFound();
            }
            await this.materialTypeRepository.DeleteAsync(weekDay);
            return RedirectToAction(nameof(Index));
        }
    }
}
