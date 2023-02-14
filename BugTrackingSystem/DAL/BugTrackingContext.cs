using Microsoft.EntityFrameworkCore;
using BugTrackingSystem.Models;

namespace BugTrackingSystem.DAL
{
    public class BugTrackingContext : DbContext
    {
        public BugTrackingContext(DbContextOptions<BugTrackingContext> options) : base(options) { }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<ProjectBug> ProjectBugs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<BugMessage> BugMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable(nameof(Project));
            modelBuilder.Entity<Bug>().ToTable(nameof(Bug));
            modelBuilder.Entity<ProjectBug>().ToTable(nameof(ProjectBug))
                .HasKey(pb => new { pb.ProjectId, pb.BugId });
            modelBuilder.Entity<Message>().ToTable(nameof(Message));
            modelBuilder.Entity<BugMessage>().ToTable(nameof(BugMessage))
                .HasKey(bm => new { bm.BugId, bm.MessageId });
        }
    }
}
