using System.Text;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cine.Shared.Infrastructure.Events;

public class RabbitMqEventsBusBackgroundService : IEventsBus, IHostedService
{
    private readonly string _hostName;
    private readonly int _port;
    private readonly string _exchange;

    private IConnection _connection;
    private IChannel _channel;

    public RabbitMqEventsBusBackgroundService(string hostName = "host.docker.internal", int port = 5672,
        string exchange = "integration_events")
    {
        _hostName = hostName;
        _port = port;
        _exchange = exchange;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionFactory = new ConnectionFactory { HostName = _hostName, Port = _port };
        _connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.ExchangeDeclareAsync(exchange: _exchange, type: ExchangeType.Fanout,
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

        await _channel.BasicPublishAsync(exchange: _exchange, routingKey: "", mandatory: true,
            basicProperties: properties, body: Encoding.UTF8.GetBytes(body), cancellationToken: cancellationToken);
    }

    public async Task SubscribeAsync<TEvent>(IIntegrationEventHandler<TEvent> handler,
        CancellationToken cancellationToken = default) where TEvent : IIntegrationEvent
    {
        var queue = await _channel.QueueDeclareAsync(cancellationToken: cancellationToken);

        var queueName = queue.QueueName;
        await _channel.QueueBindAsync(queue: queueName, exchange: _exchange, routingKey: "",
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, args) =>
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
        };

        await _channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer,
            cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.CloseAsync();
        await _channel.CloseAsync();

        await _connection.DisposeAsync();
        await _channel.DisposeAsync();
    }
}