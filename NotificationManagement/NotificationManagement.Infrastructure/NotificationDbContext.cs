using Microsoft.EntityFrameworkCore;
using NotificationManagement.Domain.Entities;

namespace NotificationManagement.Infrastructure
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Recipient).HasMaxLength(100);
            });
        }
    }
}
