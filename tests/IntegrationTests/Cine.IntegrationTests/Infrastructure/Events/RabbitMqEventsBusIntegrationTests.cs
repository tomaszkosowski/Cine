using Cine.Shared.Infrastructure.Events;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Testcontainers.RabbitMq;

namespace Cine.IntegrationTests.Infrastructure.Events;

public class RabbitMqEventsBusIntegrationTests : IAsyncLifetime
{
    private readonly RabbitMqContainer _container;
    private IHostedService _hostedService;
    private IEventsBus _eventsBus;

    public RabbitMqEventsBusIntegrationTests()
    {
        _container = new RabbitMqBuilder("rabbitmq:3-management-alpine")
            .WithName("rabbitmq-integration-tests")
            .WithPortBinding(5672, true)
            .WithUsername("guest")
            .WithPassword("guest")
            .Build();
    }

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();

        var eventsBus = new RabbitMqEventsBusBackgroundService(_container.GetConnectionString());
            
        _hostedService = eventsBus;
        _eventsBus = eventsBus;
            
        await _hostedService.StartAsync(CancellationToken.None);
    }

    public async ValueTask DisposeAsync() => await _container.DisposeAsync();

    [Fact]
    public async Task Publish_WithValidIntegrationEvent_ShouldHandleIntegrationEvent()
    {
        // Arrange
        var manualResetEvent = new ManualResetEventSlim();

        var integrationEvent = new TestIntegrationEvent(Guid.NewGuid());
        var integrationEventHandler = new TestIntegrationEventHandler(manualResetEvent);

        await _eventsBus.SubscribeAsync("", integrationEventHandler, TestContext.Current.CancellationToken);

        // Act
        await _eventsBus.PublishAsync(integrationEvent, TestContext.Current.CancellationToken);

        // Assert
        manualResetEvent.Wait(TimeSpan.FromSeconds(1), TestContext.Current.CancellationToken);
        integrationEvent.ValidationId.Should().Be(integrationEventHandler.ReceivedEvent!.ValidationId);
    }

    #region Embedded

    record TestIntegrationEvent(Guid ValidationId): IntegrationEvent;

    class TestIntegrationEventHandler(ManualResetEventSlim manualResetEvent) : IIntegrationEventHandler<TestIntegrationEvent>
    {
        public TestIntegrationEvent? ReceivedEvent { get; private set; }

        public Task HandleAsync(TestIntegrationEvent @event)
        {
            manualResetEvent.Set();
            ReceivedEvent = @event;

            return Task.CompletedTask;
        }
    }

    #endregion
}