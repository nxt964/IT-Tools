using Microsoft.AspNetCore.Mvc;
using ITtools_clone.Models;
using ITtools_clone.Services;
using Microsoft.AspNetCore.Http;

public class AuthController : Controller
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string Password)
    {
        var user = _userService.GetUserByEmail(email);
        if (user == null)
        {
            ViewBag.Error = "Email không tồn tại!";
            return View();
        }

        if (!_userService.ValidateUserLogin(email, Password))
        {
            ViewBag.Error = "Mật khẩu không chính xác!";
            return View();
        }

        HttpContext.Session.SetInt32("UserId", user.usid);
        HttpContext.Session.SetString("Username", user.username);
        HttpContext.Session.SetInt32("Premium", user.premium ? 1 : 0);
        HttpContext.Session.SetInt32("RequestPremium", user.request_premium ? 1 : 0);
        
        // If user is admin, redirect to Admin panel
        if (user.is_admin)
        {
            HttpContext.Session.SetInt32("isAdmin", 1);
            return RedirectToAction("ManageTools", "Admin");
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string Username, string Email, string Password, string ConfirmPassword)
    {
        if (Password != ConfirmPassword)
        {
            ViewBag.Error = "Mật khẩu nhập lại không khớp!";
            return View();
        }

        if (_userService.GetUserByEmail(Email) != null)
        {
            ViewBag.Error = "Email đã tồn tại!";
            return View();
        }

        if (_userService.GetUserByUsername(Username) != null)
        {
            ViewBag.Error = "Username đã tồn tại!";
            return View();
        }

        var user = new User
        {
            username = Username,
            email = Email,
            premium = false,
            is_admin = false
        };

        _userService.RegisterUser(user, Password);

        return RedirectToAction("Login");
    }

    // Đăng xuất
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
