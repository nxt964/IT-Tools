using ITtools_clone.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ToolInterface;

public class PluginController : Controller
{
    [Route("{pluginSlugName}")]
    public IActionResult LoadTool(string pluginSlugName)
    {
        var plugin = PluginLoader.GetPlugins().FirstOrDefault(p => Utils.Slugify(p.Name) == pluginSlugName);
        if (plugin == null) return NotFound("Plugin not found");

        ViewBag.PluginName = plugin.Name;
        ViewBag.PluginUI = plugin.GetUI();
        return View();
    }

    [HttpPost]
    [Route("{pluginSlugName}/execute")]
    public IActionResult ExecuteTool(string pluginSlugName, [FromBody] object inputData)
    {
        var plugin = PluginLoader.GetPlugins().FirstOrDefault(p => Utils.Slugify(p.Name) == pluginSlugName);
        if (plugin == null) return NotFound("Plugin not found");

        // Thực thi công cụ và lấy kết quả
        object result = plugin.Execute(inputData);

        return Json(new { success = true, result });
    }
}
