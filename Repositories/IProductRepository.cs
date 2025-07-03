using InventorySystem.Models;

namespace InventorySystem.Repositories;

public class IProductRepository
{
    List<Product> GetAllProducts();
    Product GetProductById(int id);
}