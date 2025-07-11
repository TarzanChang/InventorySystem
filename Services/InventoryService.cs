using InventorySystem.Models;
using InventorySystem.Repositories;

namespace InventorySystem.Services;

public class InventoryService
{
    private readonly IProductRepository _productRepository;
    
    //透過建構子，注入介面
    public InventoryService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public List<Product> GetAllProducts()
    {
        try
        {
            //呼叫介面，而非實作 (DI)
            List<Product> products = _productRepository.GetAllProducts();//呼叫介面(不是呼叫實作物件)
            if (!products.Any())
            {
                Console.WriteLine("No products found!!");
            }
            return products;
        }
        catch (Exception e)
        {
            Console.WriteLine($"讀取產品列表失敗:  {e.Message}");
            return new List<Product>();
        }
    }

    public Product GetProductById(int id)
    {
        try
        {
            Product product = _productRepository.GetProductById(id);
            if (product == null)
            {
                Console.WriteLine("No products found!!");
            }

            return product;
        }
        catch (Exception e)
        {
            Console.WriteLine($"讀取產品列表失敗:  {e.Message}");
            return new Product();
        }
    }

    public void AddProduct(string? name,decimal price,int quantity )
    {
        try
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("產品名稱不能為空!!");
            }
            //價格必須大於零
            if (price <= 0)
            {
                throw new ArgumentException("價格必須大於零!!");
            }
            //數量不能小於零
            if (quantity < 0)
            {
                throw new ArgumentException("數量不能小於零!!");
            }
            //嘗試透過 repo 新增產品
            //If Table Id column 沒有 auto_increment
            var product = new Product(_productRepository.GetNextProductId(), name, price, quantity);
            _productRepository.AddProduct(product);
            //If Table Id column 有 auto_increment
            // var productNew = new Product(name, price, quantity);
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"\n 錯誤:  {e.Message}");
        }
    }

    public void UpdateProduct(int id, string? name, decimal price, int quantity)
    {
        try
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("產品名稱不能為空!!");
            }
            //價格必須大於零
            if (price <= 0)
            {
                throw new ArgumentException("價格必須大於零!!");
            }
            //數量不能小於零
            if (quantity < 0)
            {
                throw new ArgumentException("數量不能小於零!!");
            }
            var product = new Product(id, name, price, quantity);
            _productRepository.UpdateProduct(product);
            
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"\n 錯誤:  {e.Message}");
        }
    }
}