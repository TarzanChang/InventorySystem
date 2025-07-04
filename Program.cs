using InventorySystem.Repositories;
using InventorySystem.Services;

//Server: mysql所在伺服器位置 (localhost or ip xxx.xxx.xxx.xxx)
//Port: mysql端口 (預設 3306)
//Database: inventory_db(Create Database inventory_db)
//Uid: mysql使用者名稱
//pwd: mysql使用者密碼
const string MySql_Connetion_String = "Server=localhost;Port=3306;Database=Inventory_db;Uid=admin;Pwd=&Lhq2w3e4r5TC.;";

// MySqlProductRepository mySqlProductRepository = new MySqlProductRepository(MySql_Connetion_String);
// ^MySqlProductRepository : IProductRepository
IProductRepository productRepository = new MySqlProductRepository(MySql_Connetion_String);
InventoryService inventoryService = new InventoryService(productRepository);

RunMenu();
return;

void RunMenu()
{
    while (true)
    {
        DisplayMenu();
        string input = Console.ReadLine();
        switch (input)
        {
            case "1": GetAllProduct(); break;
            case "2": SearchProduct(); break;
            case "3": AddProduct(); break;
            case "0":
                Console.WriteLine("Goodbye");
                return;
        }
    }
}

void DisplayMenu()
{
    Console.WriteLine("Welcome to Inventory System!!");
    Console.WriteLine("What would you like to do?");
    Console.WriteLine("1. 查看所有產品。");
    Console.WriteLine("2. 查詢產品。");
    Console.WriteLine("3. 新增產品資訊。");
    Console.WriteLine("0. 離開。");
}

void GetAllProduct()
{
    Console.WriteLine("\n--- 所有產品列表 ---");
    var products = inventoryService.GetAllProducts();
    // var productsFromRepo = productRepository.GetAllProducts();
    Console.WriteLine("------------------------------------------");
    Console.WriteLine("ID | Name | Price |Quantity | Status");
    Console.WriteLine("------------------------------------------");
    foreach (var product in products)
    {
        Console.WriteLine(product);
    }
    Console.WriteLine("------------------------------------------");
}

void SearchProduct()
{
    Console.WriteLine("輸入欲查詢的產品編號: ");
    int input = ReadIntLine();
    var product = productRepository.GetProductById(input);
    // string? input = Console.ReadLine();
    // var product = productRepository.GetProductById(ReadInt(input));
    if (product != null)
    {
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("ID | Name | Price |Quantity | Status");
        Console.WriteLine("------------------------------------------");
        Console.WriteLine(product);
        Console.WriteLine("------------------------------------------");
        
    }
    // else
    // {
    //     Console.WriteLine("查無此產品!!請重新輸入!!");
    // }
// var product = productRepository.GetProductById(input);
    // if (product != null)
    // {
    //     Console.WriteLine("------------------------------------------");
    //     Console.WriteLine("ID | Name | Price |Quantity | Status");
    //     Console.WriteLine("------------------------------------------");
    //     Console.WriteLine(product);
    //     Console.WriteLine("------------------------------------------");
    // }
}

void AddProduct()
{
    Console.WriteLine("輸入產品名稱: ");
    string name = Console.ReadLine();
    Console.WriteLine("輸入產品價格: ");
    decimal price = ReadDecimalLine();
    Console.WriteLine("輸入產品數量: ");
    int quantity = ReadIntLine();
    productRepository.AddProduct(name, price, quantity);
}

int ReadInt(string input)
{
    try
    {
        return Convert.ToInt32(input);
    }
    catch (FormatException e)
    {
        Console.WriteLine("請輸入有效數字!!");
        return 0;
    }
}

int ReadIntLine(int defaultValue = 0)
{
    while (true)
    {
        string input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input) && defaultValue != 0)
        {
            return defaultValue;
        }
        //string parsing to int
        if (int.TryParse(input, out int value))
        {
            return value;
        }
        else
        {
            Console.WriteLine("請輸入有效數字。");
        }
    }
}

decimal ReadDecimalLine(decimal defaultValue = 0.0m)
{
    while (true)
    {
        string input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input) && defaultValue != 0.0m)
        {
            return defaultValue;
        }
        //string parsing to int
        if (decimal.TryParse(input, out decimal value))
        {
            return value;
        }
        else
        {
            Console.WriteLine("請輸入有效數字。");
        }
    }
}