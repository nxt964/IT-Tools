using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ToolInterface;
using ITtools_clone.Helpers;

namespace ITtools_clone.ViewComponents
{
    public class PluginsListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Lấy danh sách plugin
            var plugins = PluginLoader.GetPlugins();

            // Nhóm plugin theo danh mục
            var pluginCategories = plugins
                .GroupBy(p => p.Category) // Nhóm theo Category
                .ToDictionary(
                    g => g.Key, // Key là Category
                    g => g.Select(p => p.Name).ToList() // Value là danh sách plugin theo Category
                );

            return View(pluginCategories);
        }
    }
}
