using Cine.Modules.Tickets.Application.Reservations.ExpireReservation;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cine.Modules.Tickets.Infrastructure.Jobs;

internal sealed class ExpireReservationsJob(ISender sender, ILogger<ExpireReservationsJob> logger)
{
    public const string JobName = nameof(ExpireReservationsJob);

    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public async Task ExecuteAsync()
    {
        var oneOf = await sender.Send(new ExpireReservationsCommand());

        await oneOf.Match(
            count =>
            {
                logger.LogInformation("Expired {count} reservation(s)", count);
                return Task.CompletedTask;
            },
            notfound =>
            {
                logger.LogInformation("No reservations to expire");
                return Task.CompletedTask;
            },
            error => throw error.Value);
    }
}