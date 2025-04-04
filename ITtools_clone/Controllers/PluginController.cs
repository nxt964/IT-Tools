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
    private readonly IToolService _toolService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PluginController(IToolService toolService, IHttpContextAccessor httpContextAccessor)
    {
        _toolService = toolService;
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
        if (file == null || file.Length == 0)
        {
            ViewBag.Error = "Vui lòng chọn file DLL.";
            return View("AddTool");
        }

        if (!Directory.Exists(pluginPath))
        {
            Directory.CreateDirectory(pluginPath);
        }

        string filePath = Path.Combine(pluginPath, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Load the plugin to get its Name and Description
        var plugin = PluginLoader.LoadPlugin(filePath);
        if (plugin == null)
        {
            ViewBag.Error = "Không thể tải plugin.";
            return View("AddTool");
        }

        var tool = new Tool
        {
            tool_name = plugin.Name,
            description = plugin.Description,
            enabled = true,
            premium_required = false,
            category_name = plugin.Category,
            file_name = file.FileName 
        };

        try 
        {
            _toolService.AddTool(tool);
            ViewBag.Message = "Tool đã được tải lên thành công!";
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            
            // Clean up file if database save failed
            try
            {
                System.IO.File.Delete(filePath);
            }
            catch
            {
                // Ignore deletion errors
            }
        }
        
        return View("AddTool");
    }

    [Route("{pluginSlugName}")]
    public IActionResult LoadTool(string pluginSlugName)
    {
        if (_httpContextAccessor.HttpContext?.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Login", "Auth");
        }

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
