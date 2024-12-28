using Cine.Shared.Application.Database;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Cine.Shared.Application.Queries;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Movies.Application.Movies.GetMovie;

internal sealed class GetMovieQueryHandler(ISqlConnection sqlConnection, ILogger<GetMovieQueryHandler> logger)
	: IQueryHandler<GetMovieQuery,
		OneOf<
			MovieDto,
			NotFound,
			Error<ApplicationException>>>
{
	public async Task<OneOf<MovieDto, NotFound, Error<ApplicationException>>> Handle(GetMovieQuery query, CancellationToken cancellationToken)
	{
		try
		{
			const string sql = $"""
			                    SELECT	
			                    	M.[Title] AS [{nameof(MovieDto.Title)}],
			                    	M.[Description] AS [{nameof(MovieDto.Description)}],
			                    	M.[MovieGenre] AS [{nameof(MovieDto.Genre)}],
			                    	M.[Duration] AS [{nameof(MovieDto.Duration)}],
			                    	M.[ReleaseDate] AS [{nameof(MovieDto.ReleaseDate)}],
			                    	(
			                    	    SELECT 
			                    		    STRING_AGG(PD.FirstName + ' ' + PD.LastName, ', ')
			                                WITHIN GROUP (ORDER BY PD.LastName, PD.FirstName) AS FullName
			                    	    FROM [dbo].[MovieDirector] MD
			                    	    JOIN
			                    		    [dbo].[People] PD ON MD.[PersonId] = PD.[PersonId]
			                    	    WHERE MD.[MovieId] = M.[MovieId]
			                    	) AS [{nameof(MovieDto.Directors)}],
			                    	(
			                    	    SELECT 
			                    		    STRING_AGG(PC.FirstName + ' ' + PC.LastName, ', ')
			                                WITHIN GROUP (ORDER BY PC.LastName, PC.FirstName) AS FullName                                    
			                    	    FROM [dbo].[MovieCast] MC
			                    	    JOIN
			                    		    [dbo].[People] PC ON MC.[PersonId] = PC.[PersonId]
			                    	    WHERE MC.[MovieId] = M.[MovieId]
			                    	) AS [{nameof(MovieDto.Cast)}]
			                    FROM [dbo].[Movies] M
			                    WHERE M.[MovieId] = @MovieId
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