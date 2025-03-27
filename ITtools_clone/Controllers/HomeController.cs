using ITtools_clone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ITtools_clone.Services;

namespace ITtools_clone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IToolService _toolService;

        public HomeController(ILogger<HomeController> logger, IToolService toolService)
        {
            _logger = logger;
            _toolService = toolService;
        }

        public IActionResult Index()
        {
            var categorizedTools = _toolService.GetCategorizedTools();
            return View(categorizedTools);
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

        public IActionResult TestTool()
        {
            return View();
        }
    }
}
