using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain.Entities;

namespace PaymentManagement.Infrastructure
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
           : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Amount).IsRequired();
            });
        }
    }
}
