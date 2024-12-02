using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cine.Shared.Infrastructure.Events
{
    public sealed class RabbitMqEventsBus : IEventsBus, IDisposable
    {
        private readonly string _hostName;
        private readonly int _port;
        private readonly string _exchange;

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqEventsBus(string hostName = "host.docker.internal", int port = 5672, string exchange = "integration_events")
        {
            _hostName = hostName;
            _port = port;
            _exchange = exchange;

            var connectionFactory = new ConnectionFactory { HostName = _hostName, Port = _port };
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange, ExchangeType.Fanout);
        }

        public void Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IIntegrationEvent
        {
            var body = JsonConvert.SerializeObject(@event);
            
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Type = typeof(TEvent).FullName;
            
            _channel.BasicPublish(exchange: _exchange, routingKey: "", basicProperties: properties, body: Encoding.UTF8.GetBytes(body));
        }

        public void Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler) where TEvent : IIntegrationEvent
        {
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: _exchange, routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, args) =>
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

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
