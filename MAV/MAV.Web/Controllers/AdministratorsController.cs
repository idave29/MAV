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
    public class AdministratorsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper combosHelper;
        private readonly IUserHelper userHelper;
        private readonly IAdministratorRepository administratorRepository;

        public AdministratorsController(IAdministratorRepository administratorRepository,
            ICombosHelper combosHelper,
            IUserHelper userHelper)
        {
            this.administratorRepository = administratorRepository;
            this.combosHelper = combosHelper;
            this.userHelper = userHelper;
        }

        [Authorize(Roles = "Administrador")]
        // GET: Administrators
        public IActionResult Index()
        {
            return View(this.administratorRepository.GetAdministratorsWithUser());
        }

        [Authorize(Roles = "Administrador")]
        // GET: Administrators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AdministratorNotFound");
            }

            var administrator = await this.administratorRepository.GetByIdWithUserAsync(id.Value);
            if (administrator == null)
            {
                return new NotFoundViewResult("AdministratorNotFound");
            }

            return View(administrator);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var administrator = await _context.Administrators
            //    .Include(s => s.User)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (administrator == null)
            //{
            //    return NotFound();
            //}

            //return View(administrator);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Administrators/Create
        public IActionResult Create()
        {
            var model = new AdministratorViewModel
            {
                Users = combosHelper.GetComboUsers()
            };

            return View(model);
        }

        [Authorize(Roles = "Administrador")]
        // POST: Administrators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdministratorViewModel model)
        {
            if (model.UserUserName != "(Selecciona un usuario...)")
            {
                var user = await userHelper.GetUserByNameAsync(model.UserUserName);

                if (user == null)
                {
                    return new NotFoundViewResult("AdministratorNotFound");
                }

                foreach (Administrator adminTemp in administratorRepository.GetAdministratorsWithUser())
                {
                    if (adminTemp.User == user)
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe el Administrador");
                        return View(model);
                    }
                }

                var administrator = new Administrator { User = user };


                await userHelper.AddUserToRoleAsync(user, "Administrador");

                await this.administratorRepository.CreateAsync(administrator);
                return RedirectToAction(nameof(Index));
            }


            //[Bind("Id")] Administrator administrator
            //if (!ModelState.IsValid)
            //{
            //    new NotFoundViewResult("AdministratorNotFound");
            //}
            //if (ModelState.IsValid)
            //{


            //    await this.administratorRepository.CreateAsync(administrator.UserUserName);
            //    return RedirectToAction(nameof(Index));
            //}
            return View(model);
        }


        //[Authorize(Roles = "Administrador")]
        //// GET: Administrators/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new NotFoundViewResult("AdministratorNotFound");
        //    }

        //    var administrator = await this.administratorRepository.GetByIdWithUserAsync(id.Value);
        //    if (administrator.User == null)
        //    {
        //        return new NotFoundViewResult("AdministratorNotFound");
        //    }

        //    return View(administrator);
        //}

        //[Authorize(Roles = "Administrador")]
        //// POST: Administrators/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var administrator = await this.administratorRepository.GetByIdWithUserAsync(id);
        //    //var user = await this.userHelper.GetUserByIdAsync(administrator.User.Id);
        //    if (administrator.User == null)
        //    {
        //        return new NotFoundViewResult("AdministratorNotFound");
        //    }
        //    await userHelper.RemoveUserFromRoleAsync(administrator.User, "Administrador");
        //    await this.administratorRepository.DeleteAsync(administrator);
        //    //await this.userHelper.DeleteUserAsync(user);
        //    return RedirectToAction(nameof(Index));
        //}

        private bool AdministratorExists(int id)
        {
            return _context.Administrators.Any(e => e.Id == id);
        }
    }
}
