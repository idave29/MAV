namespace MAV.Web.Controllers
{
    using MAV.Web.Data;
    using MAV.Web.Data.Entities;
    using MAV.Web.Helpers;
    using MAV.Web.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using MAV.Web.Data.Repositories;
    using System.Collections.Generic;

    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IAdministratorRepository adminRep;
        private readonly IOwnerRepository ownerRep;
        private readonly IInternRepository internRep;
        private readonly ILoanRepository loanRep;
        private readonly ILoanDetailRepository loanDetailRep;
        private readonly IMaterialRepository materialRep;
        private readonly IApplicantRepository applicantRep;
        private readonly IUserHelper userHelper;
        private readonly IConfiguration configuration;
        private readonly ICombosHelper combosHelper;

        public AccountController(DataContext context, IUserHelper userHelper, IConfiguration
            configuration, ICombosHelper combosHelper, IAdministratorRepository adminRep, IOwnerRepository ownerRep, IInternRepository internRep, ILoanRepository loanRep, ILoanDetailRepository loanDetailRep, IMaterialRepository materialRep, IApplicantRepository applicantRep)
        {
            _context = context;
            this.userHelper = userHelper;
            this.configuration = configuration;
            this.combosHelper = combosHelper;
            this.adminRep = adminRep;
            this.ownerRep = ownerRep;
            this.internRep = internRep;
            this.loanRep = loanRep;
            this.loanDetailRep = loanDetailRep;
            this.materialRep = materialRep;
            this.applicantRep = applicantRep;
        }

        [Authorize(Roles = "Responsable, Administrador")]
        // GET: Statuses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }


        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await userHelper.LoginAsync(loginViewModel.Username, loginViewModel.Password, loginViewModel.RememberMe);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "User or password invalid.");
            loginViewModel.Password = string.Empty;
            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await this.userHelper.LogoutAsync();
            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody]LoginViewModel model)
        {
            if(this.ModelState.IsValid)
            {
                var user = await this.userHelper.GetUserByEmailAsync(model.Username);
                if(user!=null)
                {
                    var result = await this.userHelper.ValidatePasswordAsync(user, model.Password);
                    if(result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            this.configuration["Tokens:Issuer"], 
                            this.configuration["Tokens:Audience"],
                            claims,
                            expires:DateTime.UtcNow.AddDays(15),
                            signingCredentials:credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };
                        return this.Created(string.Empty, results);
                    }

                }
            }
            return BadRequest();
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        // GET: Administrators/Details/5
        public async Task<IActionResult> Details(string Id)
        {
            if (Id == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == Id);

            if (user == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            if (!this.User.IsInRole("Responsable") && !this.User.IsInRole("Administrador") && user.UserName != this.User.Identity.Name)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            return View(user);
        }

        [HttpGet]
        // GET: Administrators/Details/5
        public async Task<IActionResult> DetailsA(string id)
        {
            var userName = id.ToString();
            var user = await userHelper.GetUserByNameAsync(userName);

            if (user == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            if (!this.User.IsInRole("Responsable") && !this.User.IsInRole("Administrador") && userName != this.User.Identity.Name)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            return RedirectToAction("Details", "Account", new { id = user.Id });
        }

        [Authorize(Roles = "Responsable, Administrador")]
        public IActionResult Register()
        {
            var model = new RegisterViewModel { Roles = combosHelper.GetComboRoles(), Types = combosHelper.GetComboApplicantTypes() };

            return View(model);
        }

        [Authorize(Roles = "Responsable, Administrador")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userHelper.GetUserByNameAsync(model.Email.ToString());

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        UserName = model.Email.ToString(),                        
                    };

                    var result = await userHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "El usuario no se pudo crear");
                        return View(model);
                    }

                    if (ModelState.IsValid)
                    {
                        if (model.RoleName != "0")
                        {
                            await userHelper.AddUserToRoleAsync(user, model.RoleName);
                        }

                        switch (model.RoleName)
                        {
                            case "Administrador":
                                _context.Administrators.Add(new Administrator { User = user });
                                break;
                            case "Becario":
                                _context.Interns.Add(new Intern { User = user  });
                                break;
                            case "Responsable":
                                _context.Owners.Add(new Owner { User = user });
                                break;
                            case "Solicitante":
                                var applicantType = _context.ApplicantTypes.FirstOrDefault(m => m.Id == model.TypeId);
                                _context.Applicants.Add(new Applicant { User = user, ApplicantType = applicantType, Debtor = false });
                                break;
                            default:
                                applicantType = _context.ApplicantTypes.FirstOrDefault(m => m.Id == model.TypeId);
                                _context.Applicants.Add(new Applicant { User = user, ApplicantType = applicantType, Debtor = false });
                                await userHelper.AddUserToRoleAsync(user, "Solicitante");
                                break;
                        }

                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index", "Account");
                    }
                }

                ModelState.AddModelError(string.Empty, "El usuario ya existe");
            }

            return View(model);
        }

        [Authorize(Roles = "Responsable, Administrador")]
        // GET: Administrators/Delete/5
        public async Task<IActionResult> Delete(string Id)
        {
            if (Id == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == Id);



            if (user == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            return View(user);
        }

        [Authorize(Roles = "Responsable, Administrador")]
        // POST: Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {   
                //Buscamos al usuario en la tabla User
                var user = await this.userHelper.GetUserByIdAsync(id);
                if (user == null)
                {
                    return new NotFoundViewResult("UserNotFound");
                }

                //Agregamos en una lista los roles 
                IList<string> roles = await userHelper.GetRolesAsync(user);

                foreach (string rol in roles)
                {
                    if (rol == "Administrador") //Si es administardor, se borrara desde la tabla de admin
                    {
                        var adminWithUser = await this.adminRep.GetByIdUserWithUserAdminAsync(id);
                        if (adminWithUser != null)
                            await this.adminRep.DeleteAsync(adminWithUser);
                    }
                    if (rol == "Responsable") 
                    {
                        var ownerWithUser = await this.ownerRep.GetByIdUserOwnerWithUserAsync(id);
                        if (ownerWithUser != null)
                            await this.ownerRep.DeleteAsync(ownerWithUser);

                        //Agregar metodo para quitar material
                    }
                    if (rol == "Solicitante" || rol == "Becario") 
                    {
                        var loanDetailUser = await this.loanDetailRep.GetByIdAppOrInternLoanDetailsAsync(id);
                        if(loanDetailUser != null)
                            await this.loanDetailRep.DeleteAsync(loanDetailUser);
                        //

                        var loanUser = await this.loanRep.GetByIdAppOrInternLoansAsync(id);
                        if (loanUser != null)
                            await this.loanRep.DeleteAsync(loanUser);

                        if (rol == "Solicitante")
                        {
                            var applicantWithUser = await   this.applicantRep.GetByIdUserWithUserApplicantAsync(id);
                            if (applicantWithUser != null)
                                await this.applicantRep.DeleteAsync(applicantWithUser);
                        }
                        if (rol == "Becario") 
                        {
                            var internWithUser = await this.internRep.GetByIdUserInternWithUserAsync(id);
                            if (internWithUser != null)
                                await this.internRep.DeleteAsync(internWithUser);
                        }
                    }

                }

                await this.userHelper.DeleteUserAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Administrators/Edit/5
        public async Task<IActionResult> Edit(string Id)
        {

            if (Id == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == Id);

            if (user == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            var model = new User
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email

            };

            if (!this.User.IsInRole("Responsable") && !this.User.IsInRole("Administrador") && user.UserName != this.User.Identity.Name)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            return View(model);
        }


        //POST: Administrators/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, User model)
        {
            if (id != model.Id)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            if (ModelState.IsValid)
            {

                var user = await userHelper.GetUserByIdAsync(id);

                if (user == null)
                {
                    return new NotFoundViewResult("UserNotFound");
                }

                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                //await _context.SaveChangesAsync();

                await this.userHelper.UpdateUserAsync(user);

                return RedirectToAction("Details", "Account", new {id = user.Id});

            }

            return View(model);
        }

    }
}
