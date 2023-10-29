using Microsoft.EntityFrameworkCore;

namespace UserApi.Entities
{
    public class UserIdentityDbContext : DbContext
    {
        public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(u => u.FirstName).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired().HasMaxLength(60);
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(254);
        }
    }
}
