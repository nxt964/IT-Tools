using Microsoft.EntityFrameworkCore;
using ITtools_clone.Repositories;
using ITtools_clone.Services;
using ITtools_clone.Models;
using ITtools_clone.Middlewares;
// using ITtools_clone.Middleware; // Removed as the namespace does not exist

namespace ITtools_clone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // Lấy chuỗi kết nối từ appsettings.json
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

            // Đăng ký Entity Framework Core với MySQL
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Cấu hình Session
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();

            // Register repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IToolRepository, ToolRepository>();

            // Register services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IToolService, ToolService>();

            var app = builder.Build();

            // Kiểm tra kết nối đến MySQL TRƯỚC KHI CHẠY ỨNG DỤNG
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                try
                {
                    if (dbContext.Database.CanConnect())
                    {
                        Console.WriteLine("Kết nối MySQL thành công!");
                    }
                    else
                    {
                        Console.WriteLine("⚠ Không thể kết nối đến MySQL.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi kết nối MySQL: {ex.Message}");
                }
            }

            // 📌 Cấu hình pipeline HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // Use premium tool access middleware
            app.UseMiddleware<PremiumToolAccessMiddleware>();
            app.UseSession(); // Kích hoạt session
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}
