using Cine.Modules.Movies.Domain;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Cine.Modules.Movies.Infrastructure.Database.Write
{
    internal class PeopleRespository(WriteContext _context) : IPeopleRepository
    {
        public async Task AddAsync(Person person)
        {
            await _context.AddAsync(person);
        }

        public async Task<IReadOnlyList<Person>> GetAsync(IReadOnlyList<(string FirstName, string LastName)> fullNames)
        {
            var predicate = PredicateBuilder.New<Person>();

            foreach (var (firstName, lastName) in fullNames)
            {
                predicate = predicate.Or(person => person.FirstName == firstName && person.LastName == lastName);
            }

            var people = await _context.People.Where(predicate).ToListAsync();
            return people.AsReadOnly();
        }
    }
}
