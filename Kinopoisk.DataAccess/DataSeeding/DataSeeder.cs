using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kinopoisk.DataAccess.DataSeeding;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<KinopoiskContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        if (await context.Films.AnyAsync())
            return;

        var genres = await SeedGenres(context); 
        var directors = await SeedDirectors(context);
        var actors = await SeedActors(context);
        var films = await SeedFilms(context);
        
        await SeedUsers(userManager);

        await SeedActorRoles(context);
    }
    private async static Task<List<Genre>> SeedGenres(KinopoiskContext context)
    {
        if (context.Genres.Any())
            return new List<Genre>();

        var genres = new List<Genre>
        {
            new() { Name = "Action" },
            new() { Name = "Drama" },
            new() { Name = "Comedy" },
            new() { Name = "Thriller" },
            new() { Name = "Sci-Fi" }
        };
        await context.Genres.AddRangeAsync(genres);
        await context.SaveChangesAsync();
        return genres;
    }
    private async static Task<List<Actor>> SeedActors(KinopoiskContext context)
    {
        if (context.Actors.Any())
            return new List<Actor>();

        var actors = new List<Actor>
        {
            new() { Name = "Leonardo DiCaprio" },
            new() { Name = "Matthew McConaughey" },
            new() { Name = "Keanu Reeves" },
            new() { Name = "Brad Pitt" },
            new() { Name = "Edward Norton" },
            new() { Name = "Joseph Gordon-Levitt" },
            new() { Name = "Elliot Page" },
            new() { Name = "Anne Hathaway" },
            new() { Name = "Hugo Weaving" }
        };

        await context.Actors.AddRangeAsync(actors);
        await context.SaveChangesAsync();
        return actors;
    }
    private async static Task SeedActorRoles(KinopoiskContext context)
    {
        var interstellar = context.Films.FirstOrDefault(f => f.Name == "Interstellar");
        var inception = context.Films.FirstOrDefault(f => f.Name == "Inception");
        var theMatrix = context.Films.FirstOrDefault(f => f.Name == "The Matrix");
        var fightClub = context.Films.FirstOrDefault(f => f.Name == "Fight Club");

        var actorRoles = new List<ActorRole>
        {
            // Inception (2010)
            new() { Actor = context.Actors.First(a => a.Name == "Leonardo DiCaprio"), Film = inception, Role = FilmRole.Main },
            new() { Actor = context.Actors.First(a => a.Name == "Joseph Gordon-Levitt"), Film = inception, Role = FilmRole.Secondary },
            new() { Actor = context.Actors.First(a => a.Name == "Elliot Page"), Film = inception    , Role = FilmRole.Secondary },

            // Interstellar (2014)
            new() { Actor = context.Actors.First(a => a.Name == "Matthew McConaughey"), Film = interstellar, Role = FilmRole.Main },
            new() { Actor = context.Actors.First(a => a.Name == "Anne Hathaway"), Film = interstellar, Role = FilmRole.Secondary },

            // The Matrix (1999)
            new() { Actor = context.Actors.First(a => a.Name == "Keanu Reeves"), Film = theMatrix, Role = FilmRole.Main },
            new() { Actor = context.Actors.First(a => a.Name == "Hugo Weaving"), Film = theMatrix, Role = FilmRole.Secondary },

            // Fight Club (1999)
            new() { Actor = context.Actors.First(a => a.Name == "Brad Pitt"), Film = fightClub, Role = FilmRole.Main },
            new() { Actor = context.Actors.First(a => a.Name == "Edward Norton"), Film = fightClub, Role = FilmRole.Main }
        };
        await context.ActorRoles.AddRangeAsync(actorRoles);
        await context.SaveChangesAsync();
    }
    private async static Task SeedUsers(UserManager<User> userManager)
    {
        var usersToSeed = new List<(string username, string email)>
        {
            ("testuser", "testuser@example.com"),
            ("john_doe", "john@example.com"),
            ("alice", "alice@example.com"),
            ("bob", "bob@example.com"),
            ("charlie", "charlie@example.com")
        };

        foreach (var (username, email) in usersToSeed)
        {
            var existingUser = await userManager.FindByNameAsync(username);
            if (existingUser == null)
            {
                var user = new User
                {
                    UserName = username,
                    Email = email,
                };
                await userManager.CreateAsync(user, "Test123!");
            }
        }
    }
    private async static Task<List<Director>> SeedDirectors(KinopoiskContext context)
    {
        if (context.Directors.Any())
            return new List<Director>();

        var directors = new List<Director>
        {
            new() { Name = "Christopher Nolan" },
            new() { Name = "Lana Wachowski" },
            new() { Name = "David Fincher" },
            new() { Name = "Quentin Tarantino" }
        };

        await context.Directors.AddRangeAsync(directors);
        await context.SaveChangesAsync();
        return directors;
    }
    private async static Task<List<Film>> SeedFilms(KinopoiskContext context)
    {
        if (context.Films.Any())
            return new List<Film>();

        var nolan = await context.Directors.FirstOrDefaultAsync(d => d.Name == "Christopher Nolan");
        var wachowski = await context.Directors.FirstOrDefaultAsync(d => d.Name == "Lana Wachowski");
        var fincher = await context.Directors.FirstOrDefaultAsync(d => d.Name == "David Fincher");

        var actionGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name == "Action");
        var scifiGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name == "Sci-Fi");
        var dramaGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name == "Drama");
        var thrillerGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name == "Thriller");

        var films = new List<Film>
        {
            new()
            {
                Name = "Inception",
                Description = "A mind-bending thriller",
                Country = "USA",
                Duration = 148,
                PublishDate = new DateTime(2010, 7, 16),
                IMDBRating = 8.8,
                UsersRating = 9.0,
                Poster = "inception.jpg",
                DirectorId = nolan?.Id,
                Genres = [actionGenre, scifiGenre, dramaGenre]
            },
            new()
            {
                Name = "Interstellar",
                Description = "Exploring space and time",
                Country = "USA",
                Duration = 169,
                PublishDate = new DateTime(2014, 11, 7),
                IMDBRating = 8.6,
                UsersRating = 8.8,
                Poster = "interstellar.jpg",
                DirectorId = nolan?.Id,
                Genres = [scifiGenre, dramaGenre]
            },
            new()
            {
                Name = "The Matrix",
                Description = "Reality vs illusion",
                Country = "USA",
                Duration = 136,
                PublishDate = new DateTime(1999, 3, 31),
                IMDBRating = 8.7,
                UsersRating = 9.2,
                Poster = "matrix.jpg",
                DirectorId = wachowski?.Id,
                Genres = [actionGenre, scifiGenre, thrillerGenre]
            },
            new()
            {
                Name = "Fight Club",
                Description = "Underground fight club",
                Country = "USA",
                Duration = 139,
                PublishDate = new DateTime(1999, 10, 15),
                IMDBRating = 8.8,
                UsersRating = 8.5,
                Poster = "fightclub.jpg",
                DirectorId = fincher?.Id,
                Genres = [dramaGenre, thrillerGenre]
            }
        };

        await context.Films.AddRangeAsync(films);
        await context.SaveChangesAsync();
        return films;
    }
    
}
