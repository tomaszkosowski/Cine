namespace Cine.Modules.Movies.Domain.UnitTests.Factories;

internal static class MovieObjectFactory
{
    public static Movie CreateValidObject()
    {
        var title = "The Matrix";
        var description = "Any short description.";
        var genre = MovieGenre.Of("SciFi");
        var duration = TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(136));
        var releaseDate = new DateOnly(1999, 3, 31);

        var directors = new List<Person>
        {
            Person.Create("Larry", "Wachowski"),
            Person.Create("Andy", "Wachowski")
        };

        var crew = new List<Person>
        {
            Person.Create("Keanu", "Reeves"),
            Person.Create("Laurence", "Fishburne"),
            Person.Create("Carrie-Anne", "Moss"),
            Person.Create("Hugo", "Weaving")
        };

        return Movie.Create(title, description, genre, duration, releaseDate, directors, crew);
    }

    public static Movie CreateInvalidObject()
    {
        var title = "";
        var description = "Any short description.";
        var genre = MovieGenre.Of("SciFi");
        var duration = TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(136));
        var releaseDate = new DateOnly(1999, 3, 31);

        var directors = new List<Person>
        {
            Person.Create("Larry", "Wachowski"),
            Person.Create("Andy", "Wachowski")
        };

        var crew = new List<Person>
        {
            Person.Create("Keanu", "Reeves"),
            Person.Create("Laurence", "Fishburne"),
            Person.Create("Carrie-Anne", "Moss"),
            Person.Create("Hugo", "Weaving")
        };

        return Movie.Create(title, description, genre, duration, releaseDate, directors, crew);
    }
}