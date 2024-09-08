using Cine.Modules.Movies.Domain;

namespace Cine.Modules.Movies.Infrastructure.Database.Write
{
    internal class PeopleRespository(WriteContext _context) : IPeopleRepository
    {
        public async Task AddAsync(Person person)
        {
            await _context.AddAsync(person);
        }

        public Task<IReadOnlyList<Person>> GetAsync(IReadOnlyList<(string FirstName, string LastName)> fullNames)
        {
            throw new NotImplementedException();
        }
    }
}
