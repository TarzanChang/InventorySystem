namespace InventorySystem.Utils;

public interface INotifier
{
    void SecdNotification(string recipient,string message);
    void SendAlarm(string recipient);//庫存不足通知
}