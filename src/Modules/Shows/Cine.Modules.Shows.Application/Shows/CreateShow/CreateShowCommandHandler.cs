using System.Collections.Immutable;
using Cine.Modules.Shows.Application.Halls.GetHall;
using Cine.Modules.Shows.Application.Movies;
using Cine.Modules.Shows.Application.Movies.GetMovie;
using Cine.Modules.Shows.Application.Shows.GetShows;
using Cine.Modules.Shows.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Shows.Application.Shows.CreateShow;

public class CreateShowCommandHandler(
    ISender sender,
    IShowsRepository showRepository,
    ILogger<CreateShowCommandHandler> logger)
    : ICommandHandler<CreateShowCommand, OneOf<Guid, Error<ApplicationException>>>
{
    public async Task<OneOf<Guid, Error<ApplicationException>>> Handle(CreateShowCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var (movieOneOf, hallOneOf) = (
                await sender.Send(new GetMovieQuery(request.MovieId), cancellationToken),
                await sender.Send(new GetHallQuery(request.HallId), cancellationToken));

            hallOneOf.Switch(
                hallDto => { },
                notFound => throw new ApplicationException($"Hall with given HallId {request.HallId} was not found"),
                error => throw error.Value);

            return await movieOneOf.Match<Task<OneOf<Guid, Error<ApplicationException>>>>(
                async movieDto => await CreateShowAsync(request, movieDto, cancellationToken),
                notFound => throw new ApplicationException($"Movies with given MovieId {request.MovieId} not found"),
                error => throw error.Value);
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);

            return OneOfFactory.CreateApplicationError(ex);
        }
    }

    private async Task<OneOf<Guid, Error<ApplicationException>>> CreateShowAsync(CreateShowCommand request,
        MovieDto movieDto, CancellationToken cancellationToken)
    {
        var oneOf = await sender.Send(new GetShowsQuery(request.HallId), cancellationToken);
        return await oneOf.Match<Task<OneOf<Guid, Error<ApplicationException>>>>(
            async showDtos =>
            {
                var hallId = HallId.Create(request.HallId);
                var movieId = MovieId.Create(request.MovieId);
                var schedule = Schedule.Create(request.StartAt, movieDto.Duration);
                var otherShows = showDtos.Select(
                        showDto => new ShowInfo(
                            HallId.Create(showDto.HallId),
                            Schedule.Create(showDto.StartAt, showDto.Duration)))
                    .ToImmutableList();

                var show = Show.Create(hallId, movieId, schedule, otherShows);

                await showRepository.AddAsync(show);

                return (Guid)show.ShowId;
            },
            error => throw error.Value);
    }
}