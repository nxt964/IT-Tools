using ITtools_clone.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ToolInterface;

public class PluginController : Controller
{
  private readonly string pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");

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
