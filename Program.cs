using InventorySystem.Repositories;

//Server: mysql所在伺服器位置 (localhost or ip xxx.xxx.xxx.xxx)
//Port: mysql端口 (預設 3306)
//Database: inventory_db(Create Database inventory_db)
//Uid: mysql使用者名稱
//pwd: mysql使用者密碼
const string MySql_Connetion_String = "Server=localhost;Port=3306;Database=Inventory_db;Uid=admin;Pwd=&Lhq2w3e4r5TC.;";

MySqlProductRepository productRepository = new MySqlProductRepository(MySql_Connetion_String);

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
    Console.WriteLine("0. 離開。");
}

void GetAllProduct()
{
    Console.WriteLine("\n--- 所有產品列表 ---");
    var products = productRepository.GetAllProducts();
    if (products.Any())
    {
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("ID | Name | Price |Quantity | Status");
        Console.WriteLine("------------------------------------------");
        foreach (var product in products)
        {
            Console.WriteLine(product);
        }
    }
    Console.WriteLine("------------------------------------------");
}

void SearchProduct()
{
    Console.WriteLine("已進入 SearchProduct");
    Console.WriteLine(""); 
    Console.WriteLine(""); 
}