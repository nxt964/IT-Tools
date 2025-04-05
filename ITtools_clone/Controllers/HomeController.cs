using ITtools_clone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ITtools_clone.Services;
using Microsoft.AspNetCore.Http;

namespace ITtools_clone.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFavouriteService _favouriteService;
        private readonly IToolService _toolService;

        public HomeController(IFavouriteService favouriteService, IToolService toolService)
        {
            _favouriteService = favouriteService;
            _toolService = toolService;
        }

        public IActionResult Index()
        {
            // Check if user is admin
            bool isAdmin = HttpContext.Session.GetInt32("isAdmin") == 1;
            bool isPremiumUser = HttpContext.Session.GetInt32("Premium") == 1;
            int? userId = HttpContext.Session.GetInt32("UserId");

            var allTools = isAdmin 
                ? _toolService.GetAllTools() 
                : _toolService.GetToolsForUser(isPremiumUser);

            var favouriteTools = (userId != null) 
                ? _favouriteService.GetFavouriteToolsByUserId(userId.Value) 
                : new List<Tool>();

            var model = new HomeViewModel
            {
                AllTools = allTools,
                FavouriteTools = favouriteTools
            };

            return View(model);
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
