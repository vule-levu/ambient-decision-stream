using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

// This listener listens to everything
await channel.QueueBindAsync(queue, "ambient.stream", "#");

var lastSignal = DateTime.UtcNow;
var threshold = TimeSpan.FromSeconds(10);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (_, ea) =>
{
    if (ea.RoutingKey == "ambient.signal")
    {
        lastSignal = DateTime.UtcNow;
    }

    await Task.CompletedTask;
};

await channel.BasicConsumeAsync(
    queue: queue,
    autoAck: true,
    consumer: consumer
);

Console.WriteLine("listening for silence");

while (true)
{
    await Task.Delay(1000);

    if (DateTime.UtcNow - lastSignal > threshold)
    {
        Console.WriteLine("…");
        lastSignal = DateTime.UtcNow; // avoid spamming
    }
}
