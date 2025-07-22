using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options)
       : base(options) { }

    public DbSet<OrderManagement.Domain.Entities.Order> Orders { get; set; }
    public DbSet<OrderItem> Items { get; set; }
}

