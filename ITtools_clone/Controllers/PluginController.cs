using ITtools_clone.Helpers;
using Microsoft.AspNetCore.Mvc;
using ITtools_clone.Models;
using ITtools_clone.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class PluginController : Controller
{
    private readonly string pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
    private readonly IPluginService _pluginService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PluginController(IPluginService pluginService ,IHttpContextAccessor httpContextAccessor)
    {
        _pluginService = pluginService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public IActionResult AddTool()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadTool(IFormFile file)
    {
        var result = await _pluginService.AddPluginFromFile(file);
        
        if (result.Success)
        {
            ViewBag.Success = result.Message;
            ViewBag.Error = null;
        }
        else
        {
            ViewBag.Error = result.Message;
            ViewBag.Success = null;
        }
        
        return View("AddTool");
    }


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
