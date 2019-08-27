using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository.Contexts
{
    public class DatabaseContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=Database.db");
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<Comment>().HasKey(c => new {c.GameId, c.Id});
            builder.Entity<GameCategory>().HasKey(gc => new {gc.GameId, gc.CategoryId});

            builder.Entity<Account>().HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId);
        }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<GameCategory> GameCategories { get; set; }
    }
}
