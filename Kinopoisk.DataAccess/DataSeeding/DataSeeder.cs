using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Enitites.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kinopoisk.DataAccess.DataSeeding;

public static class DataSeeder
{
    private const string EN = "en";
    private const string RU = "ru";
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<KinopoiskContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        
        await SeedUsers(userManager, roleManager);

        if (await context.Films.AnyAsync())
            return;

        await SeedCountries(context);

        await SeedGenres(context);
        await SeedActorsAndDirectors(context);
        await SeedFilms(context);

        await SeedFilmEmployeeRoles(context);
    }
    private async static Task<List<Genre>> SeedGenres(KinopoiskContext context)
    {
        if (context.Genres.Any())
            return new List<Genre>();

        var genres = new List<Genre>
        {
            new() 
            {
                Name = new LocalizationSet
                {
                    Entity = nameof(Genre),
                    Property = nameof(Genre.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Action"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Экшен"
                        }
                    }
                },
            },
            new() 
            {
                Name = new LocalizationSet
                {
                    Entity = nameof(Genre),
                    Property = nameof(Genre.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Drama"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Драма"
                        }
                    }
                },
            },
            new() 
            { 
                Name = new LocalizationSet
                {
                    Entity = nameof(Genre),
                    Property = nameof(Genre.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Comedy"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Комедия"
                        }
                    }
                },            
            },
            new() 
            {
                Name = new LocalizationSet
                {
                    Entity = nameof(Genre),
                    Property = nameof(Genre.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Thriller"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Трилер"
                        }
                    }
                },
            },
            new() 
            {
                Name = new LocalizationSet
                {
                    Entity = nameof(Genre),
                    Property = nameof(Genre.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Sci-Fi"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Науч. фантастика"
                        }
                    }
                },
            }
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
            new() 
            {
                Name = new LocalizationSet
                {
                    Entity = nameof(FilmEmployee),
                    Property = nameof(FilmEmployee.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Leonardo DiCaprio"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Леонардо Ди Каприо"
                        }
                    }
                },
            },
            new() 
            {
                Name = new LocalizationSet
                {
                    Entity = nameof(FilmEmployee),
                    Property = nameof(Film.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Joseph Gordon-Levitt"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Джозеф Гордон-Левитт"
                        }
                    }
                },
            },
            new() 
            {
                Name = new LocalizationSet
                {
                    Entity = nameof(FilmEmployee),
                    Property = nameof(Film.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Elliot Page"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Эллиот Пейдж"
                        }
                    }
                },
            },
            new() 
            { 
                Name = new LocalizationSet
                {
                    Entity = nameof(FilmEmployee),
                    Property = nameof(Film.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Christopher Nolan"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Кристофер Нолан"
                        }
                    }
                }, 
            }
        };

        await context.FilmEmployees.AddRangeAsync(actors);
        await context.SaveChangesAsync();
        return actors;
    }
    private async static Task SeedFilmEmployeeRoles(KinopoiskContext context)
    {
        var inception
            = context.Films.FirstOrDefault(f => f.Name.Localizations.FirstOrDefault(x => x.Value == "Inception") != null);

        var nolan = context.FilmEmployees.FirstOrDefault(f => f.Name.Localizations.Any(x => x.Value == "Christopher Nolan"));

        var filmEmployeeRoles = new List<FilmEmployeeRole>
        {
            // Inception (2010)
            new() { FilmEmployee = context.FilmEmployees.First(a => a.Name.Localizations.Any(x => x.Value == "Leonardo DiCaprio")), 
                Film = inception, Role = 0, IsDirector = false },

            new() { FilmEmployee = context.FilmEmployees.First(a => a.Name.Localizations.Any(x => x.Value == "Joseph Gordon-Levitt")), 
                Film = inception, Role = 2, IsDirector = false },

            new() { FilmEmployee = context.FilmEmployees.First(a => a.Name.Localizations.Any(x => x.Value == "Elliot Page")), 
                Film = inception    , Role = 1 , IsDirector = false},

            new() { FilmEmployee = nolan, Film = inception, Role = 0, IsDirector = true },
        };
        await context.FilmEmployeeRoles.AddRangeAsync(filmEmployeeRoles);
        await context.SaveChangesAsync();
    }
    private async static Task SeedUsers(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        var roles = new[] { "admin", "user" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }

        var usersToSeed = new List<(string username, string email)>
        {
            ("testuser", "testuser@example.com"),
            ("john_doe", "john@example.com"),
            ("alice", "alice@example.com"),
            ("bob", "bob@example.com"),
            ("charlie", "charlie@example.com")
        };

        for (int i = 0; i < usersToSeed.Count; i++)
        {
            var (username, email) = usersToSeed[i];
            var existingUser = await userManager.FindByNameAsync(username);

            if (existingUser == null)
            {
                var user = new User
                {
                    UserName = username,
                    Email = email
                };

                var result = await userManager.CreateAsync(user, "Test123!");

                if (result.Succeeded)
                {
                    var role = i == 0 ? "admin" : "user";
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
    private async static Task<List<Film>> SeedFilms(KinopoiskContext context)
    {
        if (context.Films.Any())
            return new List<Film>();

        var actionGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name.Localizations.FirstOrDefault(x => x.Value == "Action") != null);
        var scifiGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name.Localizations.FirstOrDefault(x => x.Value == "Sci-Fi") != null);
        var dramaGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name.Localizations.FirstOrDefault(x => x.Value == "Drama") != null);
        var thrillerGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name.Localizations.FirstOrDefault(x => x.Value == "Thriller") != null);

        var usa = await context.Countries.FirstOrDefaultAsync(c => c.Name.Localizations.FirstOrDefault(x => x.Value == "USA") != null);

        var films = new List<Film>
        {
            new()
            {
                Name = new LocalizationSet
                {
                    Entity = nameof(Film),
                    Property = nameof(Film.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Inception"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Начало"
                        }
                    }
                },
                Description = new LocalizationSet
                {
                    Entity = nameof(Film),
                    Property = nameof(Film.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "A thief who steals corporate secrets through the use of dream-sharing technology " +
                            "is given the inverse task of planting an idea into the mind of a C.E.O., " +
                            "but his tragic past may doom the project and his team to disaster."
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Профессиональные воры внедряются в сон наследника огромной империи."
                        }
                    }
                },
                Country = usa,
                Duration = 148,
                PublishDate = new DateTime(2010, 7, 16),
                IMDBRating = 8.8,
                Poster = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_FMjpg_UX1000_.jpg",
                Genres = [
                    new FilmGenre { GenreId = actionGenre.Id },
                    new FilmGenre { GenreId = scifiGenre.Id },
                    new FilmGenre { GenreId = thrillerGenre.Id },
                    new FilmGenre { GenreId = dramaGenre.Id },
                    ]
            },
        };

        await context.Films.AddRangeAsync(films);
        await context.SaveChangesAsync();
        return films;
    }
    private async static Task SeedCountries(KinopoiskContext context)
    {
        if (context.Countries.Any())
            return;
        var countries = new List<Country>
        {
            new() 
            { 
                Name = new LocalizationSet
                {
                    Entity = nameof(Country),
                    Property = nameof(Country.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "USA"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "США"
                        }
                    }
                }, 
                IsoCode = "us" 
            },
            new() 
            { 
                Name = new LocalizationSet
                {
                    Entity = nameof(Country),
                    Property = nameof(Country.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Great Britain"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Великобритания"
                        }
                    }
                },
                IsoCode = "gb" 
            },
            new() 
            {
                Name = new LocalizationSet
                {
                    Entity = nameof(Country),
                    Property = nameof(Country.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Canada"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Канада"
                        }
                    }
                },                
                IsoCode = "ca" 
            },
            new() 
            {
                Name = new LocalizationSet
                {
                    Entity = nameof(Country),
                    Property = nameof(Country.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Australia"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Австралия"
                        }
                    }
                },
                IsoCode = "au" 
            },
            new() 
            {
                Name = new LocalizationSet
                {
                    Entity = nameof(Country),
                    Property = nameof(Country.Name),
                    Localizations = new()
                    {
                        new Localization
                        {
                            CultureInfo = EN,
                            Value = "Germany"
                        },
                        new Localization
                        {
                            CultureInfo = RU,
                            Value = "Германия"
                        }
                    }
                },
                IsoCode = "de" 
            }
        };
        await context.AddRangeAsync(countries);
        await context.SaveChangesAsync();
    }
}
