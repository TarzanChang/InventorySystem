namespace InventorySystem.Utils;

public class EmailNotifier : INotifier
{
    public void SendEmail(string recipient, string message)
    {
        Console.WriteLine($"發送 Email 至 {recipient}: {message}");
        //發送email 邏輯實作
    }

    public void SecdNotification(string recipient, string message)
    {
        Console.WriteLine($"<UNK> {recipient}: {message}");
    }

    public void SendAlarm(string recipient)
    {
        Console.WriteLine($"{recipient}");
    }
}