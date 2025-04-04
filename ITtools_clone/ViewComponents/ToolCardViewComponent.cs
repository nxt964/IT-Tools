using ITtools_clone.Models;
using ITtools_clone.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITtools_clone.ViewComponents
{
    public class ToolCardViewComponent : ViewComponent
    {
        private readonly IFavouriteService _favouriteService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ToolCardViewComponent(IFavouriteService favouriteService, IHttpContextAccessor httpContextAccessor)
        {
            _favouriteService = favouriteService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke(Tool tool)
        {
            var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId") ?? -1;
            var isUserFavourite = _favouriteService.IsFavourite(userId, tool.tid);

            return View((tool, isUserFavourite));
        }
        
    }
}