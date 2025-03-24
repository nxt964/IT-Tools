
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
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.usid)
                .IsUnique(); 

            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.UserId, f.ToolId }); // Thiết lập khóa chính tổng hợp

            modelBuilder.Entity<Favorite>()
                .HasOne<User>()  // Thiết lập quan hệ với User
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorite>()
                .HasOne<Tool>()  // Thiết lập quan hệ với Tool
                .WithMany()
                .HasForeignKey(f => f.ToolId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
