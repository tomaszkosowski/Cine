using Cine.Shared.Infrastructure.Events;
using FluentAssertions;
using Testcontainers.RabbitMq;

namespace Cine.IntegrationTests.Infrastructure.Events
{
    public class RabbitMqEventsBusIntegrationTests : IAsyncLifetime
    {
        private readonly RabbitMqContainer _container;
        private readonly IEventsBus _eventsBus;

        public RabbitMqEventsBusIntegrationTests()
        {
            _container = _container = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management-alpine")
                .WithName("rabbitmq-integration-tests")
                .Build();

            _eventsBus = new RabbitMqEventsBus(_container.Hostname);
        }

        public async Task InitializeAsync() => await _container.StartAsync();

        public async Task DisposeAsync() => await _container.DisposeAsync();

        [Fact]
        public void Publish_WithValidIntegrationEvent_ShouldHandleIntegrationEvent()
        {
            // Arrange
            var manualResetEvent = new ManualResetEventSlim();

            var integrationEvent = new TestIntegrationEvent(Guid.NewGuid());
            var integrationEventHandler = new TestIntegrationEventHandler(manualResetEvent);

            _eventsBus.Subscribe(integrationEventHandler);

            // Act
            _eventsBus.Publish(integrationEvent);

            // Assert
            manualResetEvent.Wait(TimeSpan.FromSeconds(1));
            integrationEvent.Id.Should().Be(integrationEventHandler.ReceivedEvent!.Id);
        }

        #region Embedded

        record TestIntegrationEvent(Guid Id) : IntegrationEvent(Id);

        class TestIntegrationEventHandler(ManualResetEventSlim _manualResetEvent) : IIntegrationEventHandler<TestIntegrationEvent>
        {
            public TestIntegrationEvent? ReceivedEvent { get; private set; }

            public Task HandleAsync(TestIntegrationEvent @event)
            {
                _manualResetEvent.Set();
                ReceivedEvent = @event;

                return Task.CompletedTask;
            }
        }

        #endregion
    }
}
