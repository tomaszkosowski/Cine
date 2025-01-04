using Cine.Shared.Application.Database;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Cine.Shared.Application.Queries;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Shows.Application.Movies.GetMovie;

public class GetMovieQueryHandler(ISqlConnection sqlConnection, ILogger<GetMovieQueryHandler> logger)
    : IQueryHandler<GetMovieQuery,
        OneOf<
            MovieDto,
            NotFound,
            Error<ApplicationException>>>
{
    public async Task<OneOf<MovieDto, NotFound, Error<ApplicationException>>> Handle(GetMovieQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            const string sql = $"""
                                SELECT
                                    M.[MovieId] as [{nameof(MovieDto.MovieId)}],
                                    M.[Duration] as [{nameof(MovieDto.Duration)}]
                                FROM [dbo].[Movies] M 
                                WHERE M.[MovieId] = @MovieId;
                                """;

            var movie = await sqlConnection.QuerySingleOrDefaultAsync<MovieDto>(sql, new { query.MovieId });

            return movie is null
                ? new NotFound()
                : movie;
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);

            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}