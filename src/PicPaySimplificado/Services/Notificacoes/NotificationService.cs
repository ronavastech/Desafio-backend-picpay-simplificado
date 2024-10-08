namespace PicPaySimplificado.Services.Notificacoes;

public class NotificationService : INotificacaoService
{
    public async Task SendNotification()
    {
        await Task.Delay(1000);
        Console.WriteLine("Cliente Notificado");
    }
}