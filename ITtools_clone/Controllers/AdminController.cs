using Microsoft.AspNetCore.Mvc;
using ITtools_clone.Models;
using ITtools_clone.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.IO;

namespace ITtools_clone.Controllers
{
    public class AdminController : Controller
    {
        private readonly IToolService _toolService;
        private readonly IUserService _userService;

        public AdminController(IToolService toolService, IUserService userService)
        {
            _toolService = toolService;
            _userService = userService;
        }

        // Admin Dashboard
        public IActionResult Index()
        {
            // Check if user is admin
            if (HttpContext.Session.GetInt32("isAdmin") != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            return RedirectToAction("ManageTools");
        }

        // Tool Management
        public IActionResult ManageTools()
        {
            // Check if user is admin
            if (HttpContext.Session.GetInt32("isAdmin") != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            var tools = _toolService.GetAllTools();
            return View(tools);
        }

        [HttpPost]
        public IActionResult UpdateToolStatus(int id, bool enabled)
        {
            // Check if user is admin
            if (HttpContext.Session.GetInt32("isAdmin") != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            var tool = _toolService.GetToolById(id);
            if (tool == null)
            {
                return NotFound();
            }

            tool.enabled = !tool.enabled;
            _toolService.UpdateTool(tool);
            
            return RedirectToAction("ManageTools");
        }

        [HttpPost]
        public IActionResult UpdateToolPremium(int id, bool premium)
        {
            // Check if user is admin
            if (HttpContext.Session.GetInt32("isAdmin") != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            var tool = _toolService.GetToolById(id);
            if (tool == null)
            {
                return NotFound();
            }

            tool.premium_required = !tool.premium_required;
            _toolService.UpdateTool(tool);
            
            return RedirectToAction("ManageTools");
        }

        public IActionResult DeleteTool(int id)
        {
            // Check if user is admin
            if (HttpContext.Session.GetInt32("isAdmin") != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Get tool from database
            var tool = _toolService.GetToolById(id);
            if (tool == null)
            {
                return NotFound();
            }

            try
            {
                if (!string.IsNullOrEmpty(tool.file_name))
                {
                    string pluginsPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
                    string filePath = Path.Combine(pluginsPath, tool.file_name);
                    
                    if (System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            System.IO.File.Delete(filePath);
                            Console.WriteLine($"Deleted plugin file: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error deleting file: {ex.Message}");
                            // Consider marking for later deletion if file is locked
                        }
                    }
                }

                _toolService.DeleteTool(id);

                TempData["SuccessMessage"] = "Tool and associated files deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting tool: {ex.Message}";
                Console.WriteLine($"Error in DeleteTool: {ex.Message}");
            }
            
            return RedirectToAction("ManageTools");
        }

        // User Management
        public IActionResult ManageUsers()
        {
            // Check if user is admin
            if (HttpContext.Session.GetInt32("isAdmin") != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            var users = _userService.GetAllUsers();
            return View(users);
        }

        [HttpPost]
        public IActionResult UpdateUserPremium(int id, bool premium)
        {
            // Check if user is admin
            if (HttpContext.Session.GetInt32("isAdmin") != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            user.premium = !user.premium;
            _userService.UpdateUser(user);
            
            return RedirectToAction("ManageUsers");
        }

        public IActionResult DeleteUser(int id)
        {
            // Check if user is admin
            if (HttpContext.Session.GetInt32("isAdmin") != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            _userService.DeleteUser(id);
            return RedirectToAction("ManageUsers");
        }
    }
}