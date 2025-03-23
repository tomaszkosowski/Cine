using Cine.Modules.Tickets.Application.ApiClients.Theater;
using Cine.Modules.Tickets.Application.Reservations.AddSeatToReservation;
using Cine.Modules.Tickets.Application.Reservations.CreateReservation;
using Cine.Modules.Tickets.Application.Seats;
using Cine.Modules.Tickets.Application.Shows.CreateShow;
using FluentAssertions;
using NSubstitute;
using OneOf.Types;

namespace Cine.Modules.Tickets.IntegrationTests.Reservations;

public class AddSeatToReservationTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task AddSeatToReservation_WhenValidCommand_ShouldReturnSuccess()
    {
        // Arrange
        var hallId = Guid.NewGuid();
        var showId = Guid.NewGuid();
        
        var seatId = await AddSeatAsync(hallId, showId);
        var reservationId = await AddReservationAsync(showId, hallId);
        
        var command = new AddSeatToReservationCommand(reservationId, seatId);

        // Act
        var result = await Sender.Send(command);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().BeOfType<Success>();
    }

    private async Task<Guid> AddSeatAsync(Guid hallId, Guid showId)
    {
        TheaterApiClient.GetSeatsAsync(hallId).Returns(
            new SeatsDto([new SeatDto(Guid.NewGuid(), "I", 1)]));
        
        var command = new AddSeatsCommand(hallId, showId);
        var result = await Sender.Send(command);
        
        result.IsT0.Should().BeTrue();
        return result.AsT0[0];
    }

    private async Task<Guid> AddReservationAsync(Guid showId, Guid hallId)
    {
        await AddShowAsync(showId, hallId);
        
        var command = new CreateReservationCommand(showId);
        var result = await Sender.Send(command);
        
        result.IsT0.Should().BeTrue();
        return result.AsT0;
    }

    private async Task AddShowAsync(Guid showId, Guid hallId)
    {
        var command = new CreateShowCommand(showId, hallId, DateTime.Parse("2024-01-30T12:00:00"));
        var result = await Sender.Send(command);

        result.IsT0.Should().BeTrue();
    }
}