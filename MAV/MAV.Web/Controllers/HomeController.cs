using MAV.Web.Data.Repositories;
using MAV.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MAV.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStatusRepository statusRepository;

        public HomeController(ILogger<HomeController> logger, IStatusRepository statusRepository)
        {
            this.statusRepository = statusRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<SelectListItem> list = this.statusRepository.GetComboStatuses();
            if (list == null)
            {
                return NotFound();
            }
            ViewBag.Measure = list;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
