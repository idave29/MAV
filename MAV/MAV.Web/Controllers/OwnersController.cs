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

namespace MAV.Web.Controllers
{
    public class OwnersController : Controller
    {
        private readonly DataContext _context;
        private readonly IOwnerRepository ownerRepository;
        private readonly IUserHelper userHelper;
        private readonly ICombosHelper combosHelper;
        public OwnersController(DataContext context, IOwnerRepository ownerRepository, IUserHelper userHelper, ICombosHelper combosHelper)
        {
            _context = context;
            this.ownerRepository = ownerRepository;
            this.userHelper = userHelper;
            this.combosHelper = combosHelper;
        }

        [Authorize(Roles = "Responsable, Administrador")]
        // GET: Owners
        public IActionResult Index()
        {
            return View(this.ownerRepository.GetOwnersWithUser());
        }

        [Authorize(Roles = "Responsable, Administrador")]
        // GET: Owners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnersNotFound");
            }

            var applicant = await this.ownerRepository.GetByIdOwnerWithMaterialsAsync(id.Value);
            if (applicant == null)
            {
                return new NotFoundViewResult("OwnersNotFound");
            }

            return View(applicant);
        }

        [Authorize(Roles = "Responsable, Administrador")]
        // GET: Owners/Create
        public IActionResult Create()
        {
            var model = new OwnerViewModel
            {
                Users = combosHelper.GetComboUsers()
            };

            return View(model);
        }

        [Authorize(Roles = "Responsable, Administrador")]
        // POST: Owners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OwnerViewModel model)
        {
            if (model.UserUserName != "(Selecciona un usuario...)")
            {
                var user = await userHelper.GetUserByNameAsync(model.UserUserName);

                if (user == null)
                {
                    return new NotFoundViewResult("OwnersNotFound");
                }

                foreach (Owner ownerTemp in ownerRepository.GetOwnersWithUser())
                {
                    if (ownerTemp.User == user)
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe el responsable");
                        return View(model);
                    }
                }

                var owner = new Owner { User = user };


                await userHelper.AddUserToRoleAsync(user, "Responsable");

                await this.ownerRepository.CreateAsync(owner);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        //[Authorize(Roles = "Responsable, Administrador")]
        //// GET: Owners/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new NotFoundViewResult("OwnersNotFound");
        //    }

        //    var owner = await this.ownerRepository.GetByIdOwnerWithMaterialsAsync(id.Value);

        //    if (owner == null)
        //    {
        //        return new NotFoundViewResult("OwnersNotFound");
        //    }

        //    if (owner.Materials.Count != 0)
        //    {
        //        ModelState.AddModelError(string.Empty, "This user has materials, delete them first before deleting this user");
        //        return RedirectToAction("Index", "Owners");
        //    }

        //    return View(owner);
        //}

        //[Authorize(Roles = "Responsable, Administrador")]
        //// POST: Owners/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var owner = await this.ownerRepository.GetByIdOwnerWithMaterialsAsync(id);

        //    if (owner.User == null)
        //    {
        //        return new NotFoundViewResult("OwnersNotFound");
        //    }

        //    await userHelper.RemoveUserFromRoleAsync(owner.User, "Responsable");
        //    await this.ownerRepository.DeleteAsync(owner);
        //    //await this.userHelper.DeleteUserAsync(user);
        //    return RedirectToAction(nameof(Index));
        //}

        private bool OwnerExists(int id)
        {
            return _context.Owners.Any(e => e.Id == id);
        }


        //[Authorize(Roles = "Responsable, Administrador")]
        //// GET: Owners/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var owner = await _context.Owners.FindAsync(id);
        //    if (owner == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(owner);
        //}

        //[Authorize(Roles = "Responsable, Administrador")]
        //// POST: Owners/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id")] Owner owner)
        //{
        //    if (id != owner.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(owner);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OwnerExists(owner.Id))
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
        //    return View(owner);
        //}

    }
}
