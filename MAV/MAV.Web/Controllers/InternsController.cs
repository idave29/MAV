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
using Microsoft.AspNetCore.Authorization;
using MAV.Web.Helpers;
using MAV.Web.Models;

namespace MAV.Web.Controllers
{
    public class InternsController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper userHelper;
        private readonly ICombosHelper combosHelper;
        private readonly IInternRepository internRepository;
        private readonly ILoanRepository loanRepository;
        private readonly ILoanDetailRepository loanDetailRepository;
        public InternsController(IInternRepository internRepository, IUserHelper userHelper, ICombosHelper combosHelper, ILoanRepository loanRepository, ILoanDetailRepository loanDetailRepository)
        {
            this.userHelper = userHelper;
            this.internRepository = internRepository;
            this.combosHelper = combosHelper;
            this.loanDetailRepository = loanDetailRepository;
            this.loanRepository = loanRepository;
        }

        [Authorize(Roles = "Responsable, Administrador")]
        // GET: Interns
        public IActionResult Index()
        {
            return View(this.internRepository.GetInternsWithUser());
        }

        [Authorize(Roles = "Responsable, Administrador")]
        // GET: Interns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("InternsNotFound");
            }

            var intern = await this.internRepository.GetByIdWithUserAsync(id.Value);
            if (intern == null)
            {
                return new NotFoundViewResult("InternsNotFound");
            }

            return View(intern);
        }

        // GET: Interns/Create
        public IActionResult Create()
        {
            var model = new InternViewModel
            {
                Users = combosHelper.GetComboUsers()
            };

            return View(model);
        }

        // POST: Interns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Responsable, Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InternViewModel model)
        {
            if (model.UserUserName != "(Selecciona un usuario...)")
            {
                var user = await userHelper.GetUserByNameAsync(model.UserUserName);

                if (user == null)
                {
                    return new NotFoundViewResult("InternsNotFound");
                }

                foreach (Intern internTemp in internRepository.GetInternsWithUser())
                {
                    if (internTemp.User == user)
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe el Becario");
                        return View(model);
                    }
                }

                var intern = new Intern { User = user };


                await userHelper.AddUserToRoleAsync(user, "Becario");

                await this.internRepository.CreateAsync(intern);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Interns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intern = await this.internRepository.GetByIdAsync(id.Value);
            if (intern == null)
            {
                return NotFound();
            }
            return View(intern);
        }

        // POST: Interns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Intern intern)
        {
            if (id != intern.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.internRepository.UpdateAsync(intern);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InternExists(intern.Id))
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
            return View(intern);
        }

        [Authorize(Roles = "Responsable, Administrador")]
        // GET: Interns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("InternsNotFound");
            }

            var intern = await this.internRepository.GetByIdWithUserAsync(id.Value);
            if (intern == null)
            {
                return new NotFoundViewResult("InternsNotFound");
            }

            return View(intern);
        }

        // POST: Interns/Delete/5
        [Authorize(Roles = "Responsable, Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var intern = await this.internRepository.GetByIdWithUserAsync(id);
            //var user = await this.userHelper.GetUserByIdAsync(intern.User.Id);
            if (intern.User == null)
            {
                return new NotFoundViewResult("InternsNotFound");
            }

            var loanDetailUser = await this.loanDetailRepository.GetByIdAppOrInternLoanDetailsAsync(intern.User.Id);
            if (loanDetailUser != null)
                await this.loanDetailRepository.DeleteAsync(loanDetailUser);

            var loanUser = await this.loanRepository.GetByIdAppOrInternLoansAsync(intern.User.Id);
            if (loanUser != null)
                await this.loanRepository.DeleteAsync(loanUser);

            //Agregar material para cambiarle el estatus y el loan detail se convierta en null

            await userHelper.RemoveUserFromRoleAsync(intern.User, "Becario");
            await this.internRepository.DeleteAsync(intern);
            //await this.userHelper.DeleteUserAsync(user);
            return RedirectToAction(nameof(Index));
        }

        private bool InternExists(int id)
        {
            return _context.Interns.Any(e => e.Id == id);
        }
    }
}
