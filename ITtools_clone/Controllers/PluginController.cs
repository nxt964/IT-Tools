using ITtools_clone.Helpers;
using Microsoft.AspNetCore.Mvc;
using ITtools_clone.Models;
using ITtools_clone;
using Microsoft.AspNetCore.Http;
using System.IO;
public class PluginController : Controller
{
    private readonly string pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");

    private readonly AppDbContext _context;

    public PluginController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult AddTool()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadTool(string toolName, IFormFile file)
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

        // Save the plugin details to the database
        var tool = new Tool
        {
            tool_name = plugin.Name,
            description = plugin.Description,
            enabled = true,
            premium_required = false, // Set default value for PremiumRequired
            category_name = plugin.Category
        };

        _context.Tools.Add(tool);
        _context.SaveChanges();
        
        ViewBag.Message = "Tool đã được tải lên thành công!";
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
