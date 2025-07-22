using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Interface;

public interface IInventoryRepository
{
    Inventory GetByName(string name);
    void SaveAsync();
}
