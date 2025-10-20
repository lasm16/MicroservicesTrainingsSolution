namespace UsersApi.Abstractions;

public interface IDbListener
{
    event EventHandler<string> OnNotificationReceived;
    Task StartListening(CancellationToken cancellationToken);
}