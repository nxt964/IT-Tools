using ITtools_clone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ITtools_clone.Services;
using Microsoft.AspNetCore.Http;

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
            // Check if user is premium
            bool isPremiumUser = HttpContext.Session.GetInt32("Premium") == 1;
            
            var categorizedTools = _toolService.GetCategorizedTools(isPremiumUser);
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

        public IActionResult ToolAccess(string toolName)
        {
            var tool = _toolService.GetToolByName(toolName);
            
            if (tool == null)
            {
                return NotFound();
            }
            
            // Check if tool requires premium and user is not premium
            if (tool.premium_required && HttpContext.Session.GetInt32("Premium") != 1)
            {
                return View("PremiumRequired");
            }
            
            // Continue to the tool
            return View(tool);
        }
    }
}
