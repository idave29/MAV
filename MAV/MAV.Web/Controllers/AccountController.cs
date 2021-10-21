namespace MAV.Web.Controllers
{
    using MAV.Web.Helpers;
    using Microsoft.AspNetCore.Mvc;
    public class AccountController : Controller
    {
        private readonly IUserHelper userHelper;

        public AccountController(IUserHelper userHelper)
        {
            this.userHelper = userHelper;
        }

        public IActionResult Login()
        {
            //if(this.User)
            return this.View();
        }
    }
}
