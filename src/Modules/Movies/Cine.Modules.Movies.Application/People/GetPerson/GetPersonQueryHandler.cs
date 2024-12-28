using Cine.Shared.Application.Database;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Cine.Shared.Application.Queries;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Movies.Application.People.GetPerson;

internal sealed class GetPersonQueryHandler(ISqlConnection sqlConnection, ILogger<GetPersonQueryHandler> logger)
    : IQueryHandler<GetPersonQuery,
        OneOf<
            PersonDto,
            NotFound,
            Error<ApplicationException>>>
{
    public async Task<OneOf<PersonDto, NotFound, Error<ApplicationException>>> Handle(GetPersonQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            const string sql = $"""
                                SELECT
                                    [FirstName] AS [{nameof(PersonDto.FirstName)}],
                                    [LastName] AS [{nameof(PersonDto.LastName)}]
                                FROM [dbo].[People]
                                WHERE [PersonId] = @PersonId
                                """;

            var person = await sqlConnection.QuerySingleOrDefaultAsync<PersonDto>(sql, new { query.PersonId });

            return person is null
                ? new NotFound()
                : person;
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);

            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}