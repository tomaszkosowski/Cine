using System.Text;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cine.Shared.Infrastructure.Events;

public class RabbitMqEventsBusBackgroundService(string connectionString, string exchange = "integration_events")
    : IEventsBus, IHostedService
{
    private IConnection _connection;
    private IChannel _channel;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionFactory = new ConnectionFactory { Uri = new Uri(connectionString) };
        _connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Fanout,
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
        => await DisposeAsync();

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent
    {
        var body = JsonConvert.SerializeObject(@event);

        var properties = new BasicProperties
        {
            Persistent = true,
            Type = typeof(TEvent).FullName
        };

        await _channel.BasicPublishAsync(exchange: exchange, routingKey: "", mandatory: true,
            basicProperties: properties, body: Encoding.UTF8.GetBytes(body), cancellationToken: cancellationToken);
    }

    public async Task SubscribeAsync<TEvent>(string queueName, IIntegrationEventHandler<TEvent> handler,
        CancellationToken cancellationToken = default) where TEvent : IIntegrationEvent
    {
        var queue = await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false,
            cancellationToken: cancellationToken);

        await _channel.QueueBindAsync(queue: queue.QueueName, exchange: exchange, routingKey: "",
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, args) =>
        {
            var type = args.BasicProperties.Type;
            if (type != typeof(TEvent).FullName)
            {
                return;
            }

            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var @event = JsonConvert.DeserializeObject<TEvent>(message);

            await handler.HandleAsync(@event!);

            await _channel.BasicAckAsync(args.DeliveryTag, multiple: false, cancellationToken: cancellationToken);
        };

        await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer,
            cancellationToken: cancellationToken);
    }

    private async ValueTask DisposeAsync()
    {
        await _connection.CloseAsync();
        await _channel.CloseAsync();

        await _connection.DisposeAsync();
        await _channel.DisposeAsync();
    }
}