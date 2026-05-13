using Cine.Modules.Shows.Domain;
using Cine.Modules.Shows.Domain.Events;
using Cine.Modules.Shows.IntegrationEvents.Shows;
using Cine.Shared.Infrastructure.Events;
using FluentAssertions;

namespace Cine.Modules.Shows.IntegrationTests.Shows;

public class ShowCreatedTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task ShowCreated_WhenDomainEventPublished_ShouldPublishShowCreatedIntegrationEvent()
    {
        // Arrange
        var completed = new TaskCompletionSource<bool>();
        await EventsBus.SubscribeAsync("", new IntegrationEventHandler(completed), TestContext.Current.CancellationToken);

        // Act
        await Publisher.Publish(new ShowCreatedDomainEvent(ShowId.Create(), HallId.Create(), DateTime.Parse("2025-01-30T12:00:00")), TestContext.Current.CancellationToken);

        // Assert
        var handled = await completed.Task.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
        handled.Should().BeTrue();
    }

    private class IntegrationEventHandler(TaskCompletionSource<bool> completed)
        : IIntegrationEventHandler<ShowCreatedIntegrationEvent>
    {
        public Task HandleAsync(ShowCreatedIntegrationEvent @event)
        {
            completed.TrySetResult(true);

            return Task.CompletedTask;
        }
    }
}