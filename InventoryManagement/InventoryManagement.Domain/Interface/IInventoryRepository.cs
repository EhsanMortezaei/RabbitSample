using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Interface;

public interface IInventoryRepository
{
    Inventory GetByProductName(string name);
    void SaveAsync();
}
