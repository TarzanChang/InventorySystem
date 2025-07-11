using InventorySystem.Repositories;
using InventorySystem.Services;
using InventorySystem.Utils;

//Server: mysql所在伺服器位置 (localhost or ip xxx.xxx.xxx.xxx)
//Port: mysql端口 (預設 3306)
//Database: inventory_db(Create Database inventory_db)
//Uid: mysql使用者名稱
//pwd: mysql使用者密碼
// const string MySql_Connetion_String = "Server=localhost;Port=3306;Database=Inventory_db;Uid=admin;Pwd=&Lhq2w3e4r5TC.;";
string connectionString = "";
string configFile = "appsettings.ini";

if (File.Exists(configFile))
{
    Console.WriteLine($"Reading {configFile} file.");
    try
    {
        Dictionary<string,Dictionary<string,string>> config = ReadFile(configFile);
        
        if (config.ContainsKey("Database"))
        {
            var dbConfig = config["Database"];
            connectionString =
                $"Server={dbConfig["Server"]};Port={dbConfig["Port"]};Database={dbConfig["Database"]};uid={dbConfig["Uid"]};pwd={dbConfig["Pwd"]};";
            Console.WriteLine("讀取資料庫連接字串成功!!"+connectionString);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"錯誤 : 讀取配置檔案失敗 : {e}");
        // throw;
        // connectionString = MySql_Connetion_String;
    }
}
else
{
    Console.WriteLine($"錯誤 : 配置檔案 {configFile} 不存在");
    // connectionString = MySql_Connetion_String;
}

// MySqlProductRepository mySqlProductRepository = new MySqlProductRepository(MySql_Connetion_String);
// ^MySqlProductRepository : IProductRepository
MySqlProductRepository productRepository = new MySqlProductRepository(connectionString);
MongoDBProductRepository mongo = new MongoDBProductRepository();
//小名注入 打掃阿姨1 (mysql 實作)

// IProductRepository productRepository = new MySqlProductRepository(MySql_Connetion_String);
InventoryService inventoryService = new InventoryService(productRepository);

//通知功能相關
//使用 Email 通知
EmailNotifier emailNotifier = new EmailNotifier();
NotificationService emailService = new NotificationService(emailNotifier);
//使用 Sms 通知
SmsNotifier smsNotifier = new SmsNotifier();
NotificationService smsService = new NotificationService(smsNotifier);

RunMenu();
return;

Dictionary<string,Dictionary<string,string>>  ReadFile(string filePath)
{
    var config = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
    string currentSection = "";

    foreach (string line in File.ReadLines(filePath))
    {
        string trimmedLine = line.Trim();
        if (trimmedLine.StartsWith("#") || string.IsNullOrWhiteSpace(trimmedLine))
        {
            continue; // 跳過註釋和空行
        }

        if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
        {
            currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
            config[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        else if (currentSection != "" && trimmedLine.Contains("="))
        {
            int equalsIndex = trimmedLine.IndexOf('=');
            string key = trimmedLine.Substring(0, equalsIndex).Trim();
            string value = trimmedLine.Substring(equalsIndex + 1).Trim();
            config[currentSection][key] = value;
        }
    }
    return config;
}

void RunMenu()
{
    while (true)
    {
        DisplayMenu();
        string input = Console.ReadLine();
        switch (input)
        {
            case "1": GetAllProduct(); break;
            case "2": SearchProductById(); break;
            case "3": AddProduct(); break;
            case "4": UpdateProduct(); break;
            case "5": SearchProductByName(); break;
            case "6": SearchLowQuantityProduct(); break;
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
    Console.WriteLine("2. 查詢產品ID。");
    Console.WriteLine("3. 新增產品資訊。");
    Console.WriteLine("4. 更新產品資訊");
    Console.WriteLine("5. 查詢產品名稱");
    Console.WriteLine("6. 查詢庫存偏低");
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
    emailService.NotifyUser("JOJO","查詢所有產品!!");
}

void SearchProductById()
{
    Console.WriteLine("輸入欲查詢的產品編號: ");
    int input = ReadIntLine();
    var product = inventoryService.GetProductById(input);
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
    else
    {
        Console.WriteLine("查無此產品!!請重新輸入!!");
    }
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

void SearchLowQuantityProduct()
{
    Console.WriteLine("查詢產品庫存偏低 ");
    var products = inventoryService.SearchLowQuantityProduct();

    if (products.Any())
    {
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("ID | Name | Price |Quantity | Status");
        Console.WriteLine("------------------------------------------");
        foreach (var product in products)
        {
            Console.WriteLine(product);
        }

        Console.WriteLine("------------------------------------------");
    }
}

void SearchProductByName()
{
    Console.WriteLine("查詢產品名稱關鍵字: ");
    string input = Console.ReadLine();
    var products = inventoryService.SearchProductByName(input);

    if (products.Any())
    {
        Console.WriteLine("------------------------------------------");
        Console.WriteLine($"------------查詢條件為: ({input})----------");
        Console.WriteLine("ID | Name | Price |Quantity | Status");
        Console.WriteLine("------------------------------------------");
        foreach (var product in products)
        {
            Console.Write(product);
        }

        Console.WriteLine("------------------------------------------");
    }
}

void AddProduct()
{
    Console.WriteLine("輸入產品名稱: ");
    string name = Console.ReadLine();
    Console.WriteLine("輸入產品價格: ");
    decimal price = ReadDecimalLine();
    Console.WriteLine("輸入產品數量: ");
    int quantity = ReadIntLine();
    inventoryService.AddProduct(name, price, quantity);
    // productRepository.AddProduct(name, price, quantity);
    smsService.NotifyUser("Yoyo","新增產品成功");
}

void UpdateProduct()
{
    Console.WriteLine("請輸入要更新的產品 id: ");
    int id = ReadIntLine();
    //找到對應產品
    var productSearch = inventoryService.GetProductById(id);
    if (productSearch != null)
    {
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("查到的產品為: " + productSearch);
        Console.WriteLine("------------------------------------------");
        //輸入新名稱
        Console.WriteLine("輸入更新後產品名稱: ");
        string name = Console.ReadLine();
        //輸入新價格
        Console.WriteLine("輸入更新後產品價格: ");
        decimal price = ReadDecimalLine();
        //輸入新數量
        Console.WriteLine("輸入更新後產品數量: ");
        int quantity = ReadIntLine();
        // service.UpdateProduct
        inventoryService.UpdateProduct(id, name, price, quantity);
        Console.WriteLine("更新完成!! ");
    }
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