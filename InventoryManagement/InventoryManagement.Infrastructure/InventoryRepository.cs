using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _inventoryDbContext;

    public InventoryRepository(InventoryDbContext inventoryDbContext)
    {
        _inventoryDbContext = inventoryDbContext;
    }

    public Inventory GetByName(string namne)
        => _inventoryDbContext.Inventories.FirstOrDefault(x => x.Name == namne)!;

    public void SaveAsync()
    {
        _inventoryDbContext.SaveChanges();
    }
}
