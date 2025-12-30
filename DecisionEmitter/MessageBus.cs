using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class MessageBus : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBus()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: "ambient.stream",
            type: ExchangeType.Topic,
            durable: false
        );
    }

    public void Publish(string routingKey, object message)
    {
        Console.WriteLine($"BUS [{routingKey}] {message}");
        var body = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(message)
        );

        _channel.BasicPublish(
            exchange: "ambient.stream",
            routingKey: routingKey,
            basicProperties: null,
            body: body
        );
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}
