namespace Cine.Modules.Movies.Application.People.GetPerson
{
    public record PersonDto
    {
        public required string FirstName { get; init; }

        public required string LastName { get; init; }
    }
}
