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

    public Inventory? GetByProductName(string productName)
    => _inventoryDbContext.Inventory.FirstOrDefault(x => x.ProductName == productName);


    public void SaveAsync()
    {
        _inventoryDbContext.SaveChanges();
    }
}
