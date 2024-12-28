namespace Cine.Modules.Movies.Domain;

public interface IPeopleRepository
{
    Task AddAsync(Person person);

    Task<IReadOnlyList<Person>> GetAsync(IReadOnlyList<(string FirstName, string LastName)> fullNames);
}