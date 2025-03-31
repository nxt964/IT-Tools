using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ITtools_clone.Services;

namespace ITtools_clone.ViewComponents
{
    public class PluginsListViewComponent : ViewComponent
    {
        private readonly IToolService _toolService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PluginsListViewComponent(IToolService toolService, IHttpContextAccessor httpContextAccessor)
        {
            _toolService = toolService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            // Check if user is premium
            bool isPremiumUser = _httpContextAccessor.HttpContext?.Session.GetInt32("Premium") == 1;
            
            // Get tools based on user's premium status
            var categorizedTools = _toolService.GetCategorizedTools(isPremiumUser);
            
            return View(categorizedTools);
        }
    }
}
