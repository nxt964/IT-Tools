using Microsoft.AspNetCore.Mvc;
using ITtools_clone.Services;

namespace ITtools_clone.ViewComponents
{
    public class PluginsListViewComponent : ViewComponent
    {
        private readonly IToolService _toolService;

        public PluginsListViewComponent(IToolService toolService)
        {
            _toolService = toolService;
        }

        public IViewComponentResult Invoke()
        {
            var categorizedTools = _toolService.GetCategorizedTools();
            return View(categorizedTools);
        }
    }
}
