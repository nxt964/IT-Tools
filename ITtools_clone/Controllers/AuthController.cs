using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using ITtools_clone.Models;
using ITtools_clone;
using Microsoft.AspNetCore.Http;

public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }
    // Hiển thị trang đăng nhập
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string Password)
    {
        // Tìm user theo email
        var user = _context.Users.FirstOrDefault(u => u.email == email);
        if (user == null)
        {
            ViewBag.Error = "Email không tồn tại!";
            return View();
        }

        // Kiểm tra mật khẩu
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(Password, user.password);
        if (!isPasswordValid)
        {
            ViewBag.Error = "Mật khẩu không chính xác!";
            return View();
        }

        // Lưu trạng thái đăng nhập bằng Session
        int isAdmin = 0;
        HttpContext.Session.SetInt32("UserId", user.usid);
        HttpContext.Session.SetString("Username", user.username);
        if(user.is_admin == true){
            isAdmin = 1;
        }
        HttpContext.Session.SetInt32("isAdmin", isAdmin);

        return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chủ
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

        // Kiểm tra Email đã tồn tại chưa
        if (_context.Users.Any(u => u.email == Email))
        {
            ViewBag.Error = "Email đã tồn tại!";
            return View();
        }

        // Kiểm tra Username đã tồn tại chưa
        if (_context.Users.Any(u => u.username == Username))
        {
            ViewBag.Error = "Username đã tồn tại!";
            return View();
        }

        // Hash mật khẩu trước khi lưu vào database
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

        // Tạo đối tượng User và lưu vào database
        var user = new User
        {
            username = Username,
            email = Email,
            password = hashedPassword,
            premium = false, // Set default value for Premium
            is_admin = false // Set default value for Admin
        };

        _context.Users.Add(user);
        _context.SaveChanges(); // Lưu vào database

        return RedirectToAction("Login"); // Chuyển hướng sang trang đăng nhập
    }

        // Đăng xuất
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
