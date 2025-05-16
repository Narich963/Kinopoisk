using Kinopoisk.Core.Enitites;
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

        await SeedCountries(context);

        var genres = await SeedGenres(context); 
        var actors = await SeedActorsAndDirectors(context);
        var films = await SeedFilms(context);
        
        await SeedUsers(userManager);

        await SeedFilmEmployeeRoles(context);

        await SeedComments(context, userManager);

        await SeedRating(context);
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
    private async static Task<List<FilmEmployee>> SeedActorsAndDirectors(KinopoiskContext context)
    {
        if (context.FilmEmployees.Any())
            return new List<FilmEmployee>();

        var actors = new List<FilmEmployee>
        {
            new() { Name = "Leonardo DiCaprio" },
            new() { Name = "Matthew McConaughey" },
            new() { Name = "Keanu Reeves" },
            new() { Name = "Brad Pitt" },
            new() { Name = "Edward Norton" },
            new() { Name = "Joseph Gordon-Levitt" },
            new() { Name = "Elliot Page" },
            new() { Name = "Anne Hathaway" },
            new() { Name = "Hugo Weaving" },
            new() { Name = "Lana Wachowski" },
            new() { Name = "David Fincher" },
            new() { Name = "Christopher Nolan" }
        };

        await context.FilmEmployees.AddRangeAsync(actors);
        await context.SaveChangesAsync();
        return actors;
    }
    private async static Task SeedFilmEmployeeRoles(KinopoiskContext context)
    {
        var interstellar = context.Films.FirstOrDefault(f => f.Name == "Interstellar");
        var inception = context.Films.FirstOrDefault(f => f.Name == "Inception");
        var theMatrix = context.Films.FirstOrDefault(f => f.Name == "The Matrix");
        var fightClub = context.Films.FirstOrDefault(f => f.Name == "Fight Club");

        var nolan = context.FilmEmployees.FirstOrDefault(f => f.Name == "Christopher Nolan");
        var wachowski = context.FilmEmployees.FirstOrDefault(f => f.Name == "Lana Wachowski");
        var fincher = context.FilmEmployees.FirstOrDefault(f => f.Name == "David Fincher");

        var filmEmployeeRoles = new List<FilmEmployeeRole>
        {
            // Inception (2010)
            new() { FilmEmployee = context.FilmEmployees.First(a => a.Name == "Leonardo DiCaprio"), Film = inception, Role = 0, IsDirector = false },
            new() { FilmEmployee = context.FilmEmployees.First(a => a.Name == "Joseph Gordon-Levitt"), Film = inception, Role = 2, IsDirector = false },
            new() { FilmEmployee = context.FilmEmployees.First(a => a.Name == "Elliot Page"), Film = inception    , Role = 1 , IsDirector = false},
            new() { FilmEmployee = nolan, Film = inception, Role = 0, IsDirector = true },

            // Interstellar (2014)
            new() { FilmEmployee = context.FilmEmployees.First(a => a.Name == "Matthew McConaughey"), Film = interstellar, Role = 0, IsDirector = false },
            new() { FilmEmployee = context.FilmEmployees.First(a => a.Name == "Anne Hathaway"), Film = interstellar, Role = 1 , IsDirector = false},
            new() { FilmEmployee = nolan, Film = interstellar, Role = 0, IsDirector = true },

            // The Matrix (1999)
            new() { FilmEmployee = context.FilmEmployees.First(a => a.Name == "Keanu Reeves"), Film = theMatrix, Role = 0 , IsDirector = false},
            new() { FilmEmployee = context.FilmEmployees.First(a => a.Name == "Hugo Weaving"), Film = theMatrix, Role = 1 , IsDirector = false},
            new() { FilmEmployee = wachowski, Film = theMatrix, Role = 0 , IsDirector = true},

            // Fight Club (1999)
            new() { FilmEmployee = context.FilmEmployees.First(a => a.Name == "Brad Pitt"), Film = fightClub, Role = 0 , IsDirector = false},
            new() { FilmEmployee = context.FilmEmployees.First(a => a.Name == "Edward Norton"), Film = fightClub, Role = 0 , IsDirector = false},
            new() { FilmEmployee = fincher, Film = fightClub, Role = 0 , IsDirector = true}
        };
        await context.FilmEmployeeRoles.AddRangeAsync(filmEmployeeRoles);
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
    private async static Task<List<Film>> SeedFilms(KinopoiskContext context)
    {
        if (context.Films.Any())
            return new List<Film>();

        var actionGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name == "Action");
        var scifiGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name == "Sci-Fi");
        var dramaGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name == "Drama");
        var thrillerGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name == "Thriller");

        var usa = await context.Countries.FirstOrDefaultAsync(c => c.Name == "USA");

        var films = new List<Film>
        {
            new()
            {
                Name = "Inception",
                Description = "A mind-bending thriller",
                Country = usa,
                Duration = 148,
                PublishDate = new DateTime(2010, 7, 16),
                IMDBRating = 8.8,
                Poster = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_FMjpg_UX1000_.jpg",
                Genres = [actionGenre, scifiGenre, dramaGenre]
            },
            new()
            {
                Name = "Interstellar",
                Description = "Exploring space and time",
                Country = usa,
                Duration = 169,
                PublishDate = new DateTime(2014, 11, 7),
                IMDBRating = 8.6,
                Poster = "https://m.media-amazon.com/images/M/MV5BYzdjMDAxZGItMjI2My00ODA1LTlkNzItOWFjMDU5ZDJlYWY3XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg",
                Genres = [scifiGenre, dramaGenre]
            },
            new()
            {
                Name = "The Matrix",
                Description = "Reality vs illusion",
                Country = usa,
                Duration = 136,
                PublishDate = new DateTime(1999, 3, 31),
                IMDBRating = 8.7,
                Poster = "https://m.media-amazon.com/images/M/MV5BN2NmN2VhMTQtMDNiOS00NDlhLTliMjgtODE2ZTY0ODQyNDRhXkEyXkFqcGc@._V1_.jpg",
                Genres = [actionGenre, scifiGenre, thrillerGenre]
            },
            new()
            {
                Name = "Fight Club",
                Description = "Underground fight club",
                Country = usa,
                Duration = 139,
                PublishDate = new DateTime(1999, 10, 15),
                IMDBRating = 8.8,
                Poster = "https://m.media-amazon.com/images/M/MV5BOTgyOGQ1NDItNGU3Ny00MjU3LTg2YWEtNmEyYjBiMjI1Y2M5XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg",
                Genres = [dramaGenre, thrillerGenre]
            }
        };

        await context.Films.AddRangeAsync(films);
        await context.SaveChangesAsync();
        return films;
    }
    private async static Task SeedComments(KinopoiskContext context, UserManager<User> userManager)
    {
        var films = await context.Films.ToListAsync();
        var users = await userManager.Users.ToListAsync();

        var comments = new List<Comment>
    {
        new()
        {
            Text = "This movie blew my mind!",
            CreatedAt = DateTime.UtcNow.AddDays(-3),
            FilmId = films.First(f => f.Name == "Inception").Id,
            UserId = users.First(u => u.UserName == "testuser").Id
        },
        new()
        {
            Text = "A true masterpiece!",
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            FilmId = films.First(f => f.Name == "Interstellar").Id,
            UserId = users.First(u => u.UserName == "john_doe").Id
        },
        new()
        {
            Text = "Red pill or blue pill?",
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            FilmId = films.First(f => f.Name == "The Matrix").Id,
            UserId = users.First(u => u.UserName == "alice").Id
        },
        new()
        {
            Text = "First rule of Fight Club...",
            CreatedAt = DateTime.UtcNow.AddDays(-4),
            FilmId = films.First(f => f.Name == "Fight Club").Id,
            UserId = users.First(u => u.UserName == "bob").Id
        },
        new()
        {
            Text = "Loved the visuals and soundtrack.",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            FilmId = films.First(f => f.Name == "Interstellar").Id,
            UserId = users.First(u => u.UserName == "charlie").Id
        }
    };

        if (!await context.Comments.AnyAsync())
        {
            await context.Comments.AddRangeAsync(comments);
            await context.SaveChangesAsync();
        }
    }
    private async static Task SeedCountries(KinopoiskContext context)
    {
        if (context.Countries.Any())
            return;
        var countries = new List<Country>
        {
            new() { Name = "USA", IsoCode = "us" },
            new() { Name = "UK", IsoCode = "gb" },
            new() { Name = "Canada", IsoCode = "ca" },
            new() { Name = "Australia", IsoCode = "au" },
            new() { Name = "Germany", IsoCode = "de" }
        };
        await context.AddRangeAsync(countries);
        await context.SaveChangesAsync();
    }
    private async static Task SeedRating(KinopoiskContext context)
    {
        var users = await context.Users.ToListAsync();
        var films = await context.Films.ToListAsync();

        foreach (var user in users)
        {
            foreach (var film in films)
            {
                var rating = new Rating
                {
                    UserId = user.Id,
                    FilmId = film.Id,
                    Value = Math.Round(new Random().NextDouble() * 10, 1)
                };
                await context.Ratings.AddAsync(rating);
            }
        }
        await context.SaveChangesAsync();
    }
}
