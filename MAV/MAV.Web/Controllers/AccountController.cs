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

    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper userHelper;
        private readonly IConfiguration configuration;
        private readonly ICombosHelper combosHelper;

        public AccountController(DataContext context, IUserHelper userHelper, IConfiguration
            configuration, ICombosHelper combosHelper)
        {
            _context = context;
            this.userHelper = userHelper;
            this.configuration = configuration;
            this.combosHelper = combosHelper;
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

            if (!this.User.IsInRole("Owner") && !this.User.IsInRole("Administrator") && userName != this.User.Identity.Name)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            return View(user);

            //return RedirectToAction(String.Format("Details/{0}", user.Id), "Account");
            //return RedirectToAction("Details/", "Account", user.Id );
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
                                _context.Interns.Add(new Intern { User = user });
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
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == Id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
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


        //// POST: Administrators/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id != model.Id)
        //    {
        //        return new NotFoundViewResult("UserNotFound");
        //    }

        //    if (ModelState.IsValid)
        //    {

        //        var user = await userHelper.GetUserByNameAsync(model.UserName);

        //        if (user == null)
        //        {
        //            return new NotFoundViewResult("UserNotFound");
        //        }

        //        var user2 = await userHelper.GetUserByNameAsync(model.RegistrationNumber.ToString());

        //        if (user2 != null && user2 != user)
        //        {
        //            ModelState.AddModelError(string.Empty, "Registration number already asigned");
        //            return View(model);
        //        }

        //        user2 = await userHelper.GetUserByEmailAsync(model.Email);

        //        if (user2 != null && user2 != user)
        //        {
        //            ModelState.AddModelError(string.Empty, "Email already asigned");
        //            return View(model);
        //        }



        //        user.PhoneNumber = model.PhoneNumber;
        //        user.Email = model.Email;
        //        user.RegistrationNumber = model.RegistrationNumber;
        //        user.FirstName = model.FirstName;
        //        user.LastName = model.LastName;

        //        if (model.ImageFile != null)
        //        {
        //            user.ImageURL = await imageHelper.UploadImageAsync(model.ImageFile, user.FullName, "FotosEstudiantes");
        //        }

        //        _context.Update(user);
        //        await _context.SaveChangesAsync();

        //        return RedirectToAction(String.Format("Details/{0}", user.Id), "Account");

        //    }

        //    return View(model);
        //}

    }
}
