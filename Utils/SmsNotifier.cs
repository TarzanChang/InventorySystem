namespace InventorySystem.Utils;

public class SmsNotifier : INotifier
{
    public void SecdNotification(string recipient, string message)
    {
        Console.WriteLine($"發送簡訊至{recipient}: {message}");
        //發送簡訊邏輯實作...
    }

    public void SendAlarm(string recipient)
    {
        Console.WriteLine($"{recipient}");
    }
}