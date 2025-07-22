using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options) { }

    public DbSet<Inventory> Inventories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inventory>().Property(x => x.Name).HasMaxLength(255);

        base.OnModelCreating(modelBuilder);
    }
}
