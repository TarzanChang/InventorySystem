using InventorySystem.Models;
using MySql.Data.MySqlClient;

namespace InventorySystem.Repositories;

public class MySqlProductRepository : IProductRepository
//java: implement interface
//java: extend ParentObj
{
 private readonly string _connectionString;
 public MySqlProductRepository(string connectionString)
{
 _connectionString = connectionString;
 InitializeDatabase();
}

private void InitializeDatabase()
{
 //using:以防忘記關掉, 而占用資源
 using (var connection = new MySqlConnection(_connectionString))
 {
  try
  {
   connection.Open(); //開啟連線
   string CreateTableSql = @"
        CREATE TABLE IF NOT EXISTS `products` (
            id INT PRIMARY KEY AUTO_INCREMENT,
            name VARCHAR(10),
            price  DECIMAL(10,2),
            quantity INT not null,
            status int not null
        );"; // @可以換行的字串(???), 可以跟 $"" 做比較???
   // cmd 命令: 把我們的字串(連線) 執行 connection
   using (MySqlCommand cmd = new MySqlCommand(CreateTableSql, connection))
   {
    cmd.ExecuteNonQuery(); //執行
   }

   Console.WriteLine("MySql 初始化成功或已存在!!");
  }
  catch (MySqlException e)
  {
   Console.WriteLine($"初始化失敗: {e.Message}");
  }
 }
}

public List<Product> GetAllProducts()
{
 List<Product> products = new List<Product>();
 using (var connection = new MySqlConnection(_connectionString))
 {
  connection.Open();
  string selectSql = "SELECT * FROM `products`;";
  using (MySqlCommand cmd = new MySqlCommand(selectSql, connection))
  {
   using (MySqlDataReader reader = cmd.ExecuteReader())
   {
    while (reader.Read())
        //reader = 1 box -> reader = 2 dish -> reader = 3 phone
    {
     //1. origin way
     products.Add(new Product(reader.GetInt32("id"),
                            reader.GetString("name"),
                            reader.GetDecimal("price"),
                            reader.GetInt32("quantity"))
                            {
                             Status = (Product.ProductStatus)reader.GetInt32("status")
                            });
     
     //2. obj initializer
      }
   }
  }
 }
 return products;
}

public Product GetProductById(int id)
{
 Product product = null;
 using (var connection = new MySqlConnection(_connectionString))
 {
  connection.Open();
  // string selectSql = "SELECT * FROM `products` WHERE id = " + id;
  // gen by AI ...
  string selectSql = "SELECT * FROM `products` WHERE id = @id";
  using (MySqlCommand cmd = new MySqlCommand(selectSql, connection))
  {
   // 防止sql injection...
   cmd.Parameters.AddWithValue("@id", id);
   using (MySqlDataReader reader = cmd.ExecuteReader())
   {
    if (reader.Read())
    {
     product = new Product(reader.GetInt32("id"),
      reader.GetString("name"),
      reader.GetDecimal("price"),
      reader.GetInt32("quantity"))
     {
      Status = (Product.ProductStatus)reader.GetInt32("status")
     };
    }
   }
  }
 }
 return product;
}

 public void AddProduct(Product product)
 {
  using (var connection = new MySqlConnection(_connectionString))
  {
   connection.Open();
   // string selectSql = "SELECT * FROM `products` WHERE id = " + id;
   // gen by AI ...
   //If Table Id column 沒有 auto_increment
   string insertSql = "insert into products(id,name,price,quantity,status) values(@id,@name,@price,@quantity,@status)";
   //If Table Id column 有 auto_increment
   // string insertSql = "insert into products(name,price,quantity,status) values(@name,@price,@quantity,@status)";
   using (MySqlCommand cmd = new MySqlCommand(insertSql, connection))
   {
    // 防止sql injection...
    //If Table Id column 沒有 auto_increment
    cmd.Parameters.AddWithValue("@id", product.Id);
    //If Table Id column 沒有 auto_increment 可不需要
    
    cmd.Parameters.AddWithValue("@name", product.Name);
    cmd.Parameters.AddWithValue("@price", product.Price);
    cmd.Parameters.AddWithValue("@quantity", product.Quantity);
    //todo refactor
    cmd.Parameters.AddWithValue("@status", product.Status);
    cmd.ExecuteNonQuery();
   }
  }
 }
 
//If Table Id column 沒有 auto_increment
 public int GetNextProductId()
 {
  using (var connection = new MySqlConnection(_connectionString))
  {
   connection.Open();
   string selectSql = @"SELECT IFNULL(MAX(id),0) FROM products";
   using (MySqlCommand cmd = new MySqlCommand(selectSql, connection))
   {
    var result = cmd.ExecuteScalar();
    if (result != null)
    {
     return Convert.ToInt32(result) + 1;
    }
    return 0;
   }
  }
 }
 
 public void UpdateProduct(Product product)
 {
  using (var connection = new MySqlConnection(_connectionString))
  {
   connection.Open();
   string insertSql = "update products set name = @name,price = @price,quantity = @quantity,status = @status where id = @id";
   using (MySqlCommand cmd = new MySqlCommand(insertSql, connection))
   {
    // 防止sql injection...
    cmd.Parameters.AddWithValue("@name", product.Name);
    cmd.Parameters.AddWithValue("@price", product.Price);
    cmd.Parameters.AddWithValue("@quantity", product.Quantity);
    cmd.Parameters.AddWithValue("@status", product.Status);
    cmd.Parameters.AddWithValue("@id", product.Id);
    cmd.ExecuteNonQuery();
   }
  }
 }

 public List<Product> GetProducts()
  {
   throw new NotImplementedException();
  }

}