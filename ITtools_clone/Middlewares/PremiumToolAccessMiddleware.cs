using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ITtools_clone.Services;
using ITtools_clone.Helpers;
using System.Linq;

namespace ITtools_clone.Middlewares
{
    public class PremiumToolAccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IToolService _toolService;

        public PremiumToolAccessMiddleware(RequestDelegate next, IToolService toolService)
        {
            _next = next;
            _toolService = toolService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip for non-tool URLs or admin users
            if (context.Request.Path.StartsWithSegments("/Admin") || 
                context.Request.Path.StartsWithSegments("/Auth") ||
                context.Session.GetInt32("isAdmin") == 1)
            {
                await _next(context);
                return;
            }

            // Get the path and check if it corresponds to a tool
            string path = context.Request.Path.Value?.TrimStart('/') ?? string.Empty;
            
            // Skip Home/Index and other common paths
            if (string.IsNullOrEmpty(path) || path == "Home" || path == "Home/Index")
            {
                await _next(context);
                return;
            }
            
            // Find the tool by its slugified name
            var allTools = _toolService.GetAllTools();
            var matchingTool = allTools.FirstOrDefault(t => t.tool_name != null && 
                                                        Utils.Slugify(t.tool_name) == path);
                                                        
            if (matchingTool != null && matchingTool.premium_required)
            {
                // Check if user is premium
                bool isPremiumUser = context.Session.GetInt32("Premium") == 1;
                
                if (!isPremiumUser)
                {
                    // Redirect to premium required page
                    context.Response.Redirect("/Home/PremiumRequired");
                    return;
                }
            }

            await _next(context);
        }
    }
}