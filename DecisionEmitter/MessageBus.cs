using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public sealed class MessageBus : IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public MessageBus()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.ExchangeDeclareAsync(
            exchange: "ambient.stream",
            type: ExchangeType.Topic,
            durable: false
        ).GetAwaiter().GetResult();
    }

    public void Publish(string routingKey, object message)
    {
        var body = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(message)
        );

        _channel.BasicPublishAsync(
            exchange: "ambient.stream",
            routingKey: routingKey,
            body: body
        ).GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}
