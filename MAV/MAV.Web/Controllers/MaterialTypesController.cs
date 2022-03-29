namespace MAV.Web.Controllers
{
    using MAV.Web.Data;
    using MAV.Web.Data.Entities;
    using MAV.Web.Data.Repositories;
    using MAV.Web.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class MaterialTypesController : Controller
    {
        private readonly IMaterialTypeRepository materialTypeRepository;
        private readonly DataContext _context;

        public MaterialTypesController(DataContext context, IMaterialTypeRepository materialTypeRepository)
        {
            _context = context;
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
                return new NotFoundViewResult("MaterialTypeNotFound");
            }

            var materialType = await this.materialTypeRepository.GetByIdAsync(id.Value);
            if (materialType == null)
            {
                return new NotFoundViewResult("MaterialTypeNotFound");
            }

            return View(materialType);
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
                return new NotFoundViewResult("MaterialTypeNotFound");
            }

            var materialType = await this.materialTypeRepository.GetByIdAsync(id.Value);
            if (materialType == null)
            {
                return new NotFoundViewResult("MaterialTypeNotFound");
            }
            return View(materialType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialType materialType)
        {
            if (id != materialType.Id)
            {
                return new NotFoundViewResult("MaterialTypeNotFound");
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
                        return new NotFoundViewResult("MaterialTypeNotFound");
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
                return new NotFoundViewResult("MaterialTypeNotFound");
            }

            var materialType = await this.materialTypeRepository.GetByIdMaterialTypeAsync(id.Value);
            if (materialType == null)
            {
                return new NotFoundViewResult("MaterialTypeNotFound");
            }

            if (materialType.Materials.Count != 0)
            {
                ModelState.AddModelError(string.Empty, "This type is used in one or more applicant, delete them first before deleting this.");
                return RedirectToAction("Index", "MaterialTypes");
            }

            return View(materialType);
        }

        // POST: ApplicantTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materialType = await _context.MaterialTypes.FindAsync(id);
            _context.MaterialTypes.Remove(materialType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialTypeExists(int id)
        {
            return _context.MaterialTypes.Any(e => e.Id == id);
        }

    }
}
