using Npgsql;
using UsersApi.Abstractions;

namespace UsersApi.Listeners;

public class DbNotificationListener(IConfiguration configuration) : BackgroundService, IDbListener
{
    private readonly string _connectionString = configuration.GetConnectionString("Npgsql") 
                                                ?? throw new InvalidOperationException("Connection string not found.");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(stoppingToken);

        connection.Notification += (s, e) =>
        {
            OnNotificationReceived?.Invoke(this, e.Payload);
        };

        await using var command = new NpgsqlCommand("LISTEN user_is_deleted;", connection);
        await command.ExecuteNonQueryAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await connection.WaitAsync(stoppingToken);
        }
    }

    public event EventHandler<string>? OnNotificationReceived;
    
    public async Task StartListening(CancellationToken cancellationToken)
    {
        await ExecuteAsync(cancellationToken);
    }
}