using Microsoft.EntityFrameworkCore;
using ITtools_clone.Models;

namespace ITtools_clone
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
    }
}
