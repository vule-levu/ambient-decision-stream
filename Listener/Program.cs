using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
    exchange: "ambient.stream",
    type: ExchangeType.Topic,
    durable: false
);

var queue = (await channel.QueueDeclareAsync()).QueueName;

await channel.QueueBindAsync(queue, "ambient.stream", "decision.changed");
await channel.QueueBindAsync(queue, "ambient.stream", "ambient.signal");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (_, ea) =>
{
    var body = Encoding.UTF8.GetString(ea.Body.ToArray());

    if (ea.RoutingKey == "ambient.signal")
    {
        Console.WriteLine($"{body}");
    }
    else
    {
        Console.Write(".");
    }

    await Task.CompletedTask;
};

await channel.BasicConsumeAsync(
    queue: queue,
    autoAck: true,
    consumer: consumer
);

Console.WriteLine("listening");
Console.ReadLine();
