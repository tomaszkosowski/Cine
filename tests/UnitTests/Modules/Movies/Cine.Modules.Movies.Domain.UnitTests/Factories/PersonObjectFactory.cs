namespace Cine.Modules.Movies.Domain.UnitTests.Factories
{
    internal static class PersonObjectFactory
    {
        public static Person CreateValidObject(string firstName, string lastName)
            => Person.Create(firstName, lastName);
    }
}
