namespace MAV.Web.Controllers
{
    using MAV.Web.Data;
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
        //private readonly ICombosHelper combosHelper;

        public AccountController(DataContext context, IUserHelper userHelper, IConfiguration 
            configuration)
        {
            _context = context;
            this.userHelper = userHelper;
            this.configuration = configuration;
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
        public async Task<IActionResult> DetailsActual(string id)
        {
            var userName = id;
            var user = await userHelper.GetUserByNameAsync(userName);

            if (user == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            if (!this.User.IsInRole("Responsable") && !this.User.IsInRole("Administrador") && userName != this.User.Identity.Name)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            return RedirectToAction(String.Format("Details/{0}", user.Id), "Account");
        }

    }
}
