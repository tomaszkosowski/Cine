using System.Globalization;
using Cine.Modules.Tickets.Application.Reservations.CreateReservation;
using Cine.Modules.Tickets.Application.Reservations.ExpireReservation;
using Cine.Modules.Tickets.Application.Shows.CreateShow;
using Cine.Shared.Domain;
using FluentAssertions;

namespace Cine.Modules.Tickets.IntegrationTests.Reservations;

public class ExpireReservationTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task ExpireReservation_WhenValidCommand_ShouldReturnExpiredCount()
    {
        // Arrange
        var showId = Guid.NewGuid();
        var hallId = Guid.NewGuid();

        var date = DateTime.Parse("2025-03-01T00:00:00");
        var expiryTime = TimeSpan.ParseExact(Configuration["Features:Reservations:ReservationExpiryTime"]!,
            @"hh\:mm\:ss", CultureInfo.InvariantCulture);
        
        Utc.Override(date);
        await AddReservationAsync(showId, hallId);
        
        Utc.Override(date.Add(expiryTime).AddSeconds(1));
        var command = new ExpireReservationsCommand();
        
        // Act
        var result = await Sender.Send(command);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().Be(1);
        
        Utc.Rollback();
    }

    private async Task AddReservationAsync(Guid showId, Guid hallId)
    {
        await AddShowAsync(showId, hallId);

        var command = new CreateReservationCommand(showId);
        var result = await Sender.Send(command);

        result.IsT0.Should().BeTrue();
    }

    private async Task AddShowAsync(Guid showId, Guid hallId)
    {
        var command = new CreateShowCommand(showId, hallId, DateTime.Parse("2024-01-30T12:00:00"));
        var result = await Sender.Send(command);

        result.IsT0.Should().BeTrue();
    }
}