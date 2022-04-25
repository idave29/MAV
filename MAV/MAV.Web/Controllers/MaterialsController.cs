using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MAV.Web.Data;
using MAV.Web.Data.Entities;
using MAV.Web.Data.Repositories;
using MAV.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using MAV.Web.Models;
using System.IO;

namespace MAV.Web.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly DataContext _context;
        private readonly IMaterialRepository materialRepository;
        private readonly IStatusRepository statusRepository;
        private readonly ICombosHelper combosHelper;
        private readonly IImageHelper imageHelper;

        public MaterialsController(DataContext dataContext,
            IMaterialRepository materialRepository, 
            IStatusRepository statusRepository, 
            ICombosHelper combosHelper, 
            IImageHelper imageHelper)
        {
            this.statusRepository = statusRepository;
            this.materialRepository = materialRepository;
            this._context = dataContext;
            this.combosHelper = combosHelper;
            this.imageHelper = imageHelper;
        }

        [Authorize(Roles = "Administrador")]
        // GET: Materials
        public IActionResult Index()
        {
           
            return View(this.materialRepository.GetMaterialsWithTypeWithStatusAndOwner());
        }

        // GET: Materials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("MaterialNotFound");
            }

            var material = await this.materialRepository.GetByIdWithMaterialTypeOwnerStatusAsync(id.Value); 

            if (material == null)
            {
                return new NotFoundViewResult("MaterialNotFound");
            }

            return View(material);
        }

        // GET: Materials/Create
        public IActionResult Create()
        {
            var model = new MaterialViewModel
            {
                Status = _context.Statuses.FirstOrDefault(),
                StatusId = 1,
                Statuses = combosHelper.GetComboStatuses(),
                MaterialTypes = combosHelper.GetComboMaterialTypes(), 
                Owners = combosHelper.GetComboOwners()
            };

            return View(model);
        }

        // POST: Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(MaterialViewModel model)
        {
            if (ModelState.IsValid)
            {
                var status = await _context.Statuses.FirstOrDefaultAsync(m => m.Id == model.StatusId);
                var owner = await _context.Owners.FirstOrDefaultAsync(m => m.Id == model.OwnerId);
                var materialtype = await _context.MaterialTypes.FirstOrDefaultAsync(m => m.Id == model.MaterialTypeId);

                var Material = new Material { 
                    Brand = model.Brand, 
                    Label = model.Label, 
                    MaterialModel = model.MaterialModel, 
                    Name = model.Name, 
                    SerialNum = model.SerialNum, 
                    Function = model.Function,
                    Status = status,
                    MaterialType = materialtype,
                    Owner = owner, 
                };

                if (model.ImageFile != null)
                {
                    Material.ImageURL = await imageHelper.UploadImageAsync(model.ImageFile, Material.Name, "Materiales");
                }

                _context.Add(Material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("MaterialNotFound");
            }

            var material = await this.materialRepository.GetByIdWithMaterialTypeOwnerStatusAsync(id.Value);

            if (material == null)
            {
                return new NotFoundViewResult("MaterialNotFound");
            }

            var model = new MaterialViewModel
            {
                Brand = material.Brand,
                Name = material.Name,
                MaterialModel = material.MaterialModel,
                Status = material.Status,
                Label = material.Label,
                SerialNum = material.SerialNum,
                Function = material.Function,
                ImageURL = material.ImageURL,
                StatusId = material.Status.Id,
                MaterialTypeId = material.MaterialType.Id,
                OwnerId = material.Owner.Id,
                Statuses = combosHelper.GetComboStatuses(),
                Owners = combosHelper.GetComboOwners(),
                MaterialTypes = combosHelper.GetComboMaterialTypes()
            };

            return View(model);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Label,Brand,MaterialModel,SerialNum")] Material material)
        //{
        //    if (id != material.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await this.materialRepository.UpdateAsync(material);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!await this.materialRepository.ExistAsync(material.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(material);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialViewModel model)
        {

            if (ModelState.IsValid)
            {
                var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == model.Id);

                if (material == null)
                {
                    return new NotFoundViewResult("MaterialNotFound");
                }

                material.ImageURL = model.ImageURL;
                material.Name = model.Name;
                material.Label = model.Label;
                material.Brand = model.Brand;
                material.MaterialModel = model.MaterialModel;
                material.SerialNum = model.SerialNum;
                material.Function = model.Function;

                var status = await _context.Statuses.FirstOrDefaultAsync(m => m.Id == model.StatusId);
                material.Status = status;
                var owner = await _context.Owners.FirstOrDefaultAsync(m => m.Id == model.OwnerId);
                material.Owner = owner;
                var materialType = await _context.MaterialTypes.FirstOrDefaultAsync(m => m.Id == model.MaterialTypeId);
                material.MaterialType = materialType;

                _context.Update(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("MaterialNotFound");
            }

            var material = await this.materialRepository.GetByIdWithMaterialTypeOwnerStatusAsync(id.Value);

            if (material == null)
            {
                return new NotFoundViewResult("MaterialNotFound");
            }

            if (material.Status.Name == "Prestado") //SOLO BORREN DEFECTUOSOS
            {
                ModelState.AddModelError(string.Empty, "Este material está en préstamo, elimínelos primero antes de eliminar a este material");
                return RedirectToAction("Index", "Materials");
            }

            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materials.FindAsync(id);

            //_context.Materials.Remove(material);
            material.Deleted = true;
            _context.Update(material);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }
    }
}
