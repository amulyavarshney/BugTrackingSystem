using Microsoft.EntityFrameworkCore;
using BugTrackingSystem.Models;

namespace BugTrackingSystem.DAL
{
    public class BugTrackingContext : DbContext
    {
        public BugTrackingContext(DbContextOptions<BugTrackingContext> options) : base(options) { }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable(nameof(Project));
            modelBuilder.Entity<Bug>().ToTable(nameof(Bug));
            modelBuilder.Entity<Message>().ToTable(nameof(Message));
            modelBuilder.Entity<User>().ToTable(nameof(User));
            modelBuilder.Entity<Role>().ToTable(nameof(Role));
            modelBuilder.Entity<UserRole>().ToTable(nameof(UserRole)).HasKey(ur => new { ur.UserId, ur.RoleId });
        }
    }
}
