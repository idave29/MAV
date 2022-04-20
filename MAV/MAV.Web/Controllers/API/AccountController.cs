namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
    using MAV.Web.Data;
    using MAV.Web.Data.Entities;
    using MAV.Web.Data.Repositories;
    using MAV.Web.Helpers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {

        private readonly DataContext _dataContext;
        private readonly IUserHelper userHelper;
        public AccountController(DataContext dataContext, IUserHelper userHelper)
        {
            this._dataContext = dataContext;
            this.userHelper = userHelper;
        }


        [HttpGet]
        public IActionResult GetUsers()
        {
            var a = this._dataContext.Users.Where(a => a.Deleted == false);

            if (a == null)
            {
                return null;
            }

            var x = a.Select(ar => new MAV.Common.Models.User
            {
                Id = ar.Id,
                FirstName = ar.FirstName,
                LastName = ar.LastName,
                PhoneNumber = ar.PhoneNumber,
                Email = ar.Email
                //Password = ar.User.PasswordHash
            }).ToList();

            return Ok(x); 
        }
    }
}
