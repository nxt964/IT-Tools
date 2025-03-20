using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITtools_clone.ViewComponents
{
    public class TestPluginViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var pluginCategories = new Dictionary<string, List<string>>
            {
                ["Crypto"] = new List<string> {
                    "Token Generator",
                    "Hash Text",
                    "Bcrypt",
                    "UUIDs Generator",
                    "ULID Generator",
                    "Encrypt / Decrypt", 
                },

                ["Converter"] = new List<string> {
                    "Date-time Converter", 
                    "Integer Base Converter",
                    "Roman Numeral Converter",
                    "Color Converter",
                    "Case Converter",
                },

                ["Web"] = new List<string> {
                    "Encode/Decode URL",
                    "Escape HTML Entities", 
                    "URL Parser",
                }
            };

            return View(pluginCategories);
        }
    }
}