using Microsoft.AspNetCore.Mvc;

public class AuthController : Controller
{
    // Hiển thị trang đăng nhập
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string Email, string Password)
    {
        if (Email == "admin@example.com" && Password == "123456")
        {
            return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chính nếu đăng nhập thành công
        }

        ViewBag.Error = "Email hoặc mật khẩu không đúng!";
        return View();
    }

    // Hiển thị trang đăng ký
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

        return RedirectToAction("Login"); // Chuyển hướng sang trang đăng nhập sau khi đăng ký thành công
    }
}
