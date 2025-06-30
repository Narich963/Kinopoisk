using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Kinopoisk.Core.Enitites.Localization;

namespace Kinopoisk.DataAccess.Repositories;

public class OmdbRepository : IOmdbRepository
{
    private readonly KinopoiskContext _context;

    public OmdbRepository(KinopoiskContext context)
    {
        _context = context;
    }

    public async Task<Result<Film>> ImportFilm(OmdbResponse omdbResponse)
    {
        DateTime? releaseDate = null;
        if (DateTime.TryParse(omdbResponse.Released, out var parsedDate))
        {
            releaseDate = parsedDate;
        }

        if (releaseDate.HasValue)
        {
            if (await _context.Films.AnyAsync(f => f.Name == omdbResponse.Title && f.PublishDate == releaseDate.Value))  
                return Result.Failure<Film>("Film already exists in the database.");
        }
        else
        {
            if (await _context.Films.AnyAsync(f => f.Name == omdbResponse.Title))
                return Result.Failure<Film>("Film already exists in the database.");
        }

        var genresResponse = omdbResponse.Genre?.Split(',').Select(g => g.Trim()).ToList();
        var genres = new List<FilmGenre>();
        if (genresResponse != null && genresResponse.Count > 0)
        {
            foreach (var genre in genresResponse)
            {
                var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genre);
                if (existingGenre == null)
                {
                    genres.Add(new FilmGenre
                    {
                        Genre = new Genre { Name = genre }
                    });
                }
                else
                {
                    genres.Add(new FilmGenre { GenreId = existingGenre.Id });
                }
            }
        }

        var actorsResponse = omdbResponse.Actors?.Split(',').Select(a => a.Trim()).ToList();
        var filmEmployees = new List<FilmEmployeeRole>();
        if (actorsResponse != null && actorsResponse.Count > 0)
        {
            foreach (var actor in actorsResponse)
            {
                var existingActor = await _context.FilmEmployees.FirstOrDefaultAsync(f => f.Name == actor);
                if (existingActor == null)
                {
                    filmEmployees.Add(new FilmEmployeeRole
                    {
                        FilmEmployee = new FilmEmployee { Name = actor },
                        Role = 1,
                        IsDirector = false
                    });
                }
                else
                {
                    filmEmployees.Add(new FilmEmployeeRole
                    {
                        IsDirector = false,
                        Role = 1,
                        FilmEmployeeID = existingActor.Id
                    });
                }
            }
        }

        var directorResponse = omdbResponse.Director?.Trim();
        var existingDirector = await _context.FilmEmployees.FirstOrDefaultAsync(f => f.Name == directorResponse);
        if (existingDirector == null)
        {
            filmEmployees.Add(new FilmEmployeeRole
            {
                FilmEmployee = new FilmEmployee { Name = directorResponse },
                Role = 1,
                IsDirector = true
            });
        }
        else
        {
            filmEmployees.Add(new FilmEmployeeRole
            {
                FilmEmployeeID = existingDirector.Id,
                Role = 1,
                IsDirector = true
            });
        }

        var durationString = omdbResponse.Runtime.Replace(" min", "").Trim();
        var film = new Film
        {
            Name = omdbResponse.Title,
            PublishDate = DateTime.TryParse(omdbResponse.Released, out var date) ? date : null,
            Description = new LocalizationSet
            {
                Entity = "Film",
                Property = "Description",
                Localizations = new List<Localization>()
                {
                    new Localization
                    {
                        CultureInfo = "en",
                        Value = omdbResponse.Plot
                    },
                    new Localization
                    {
                        CultureInfo = "ru",
                        Value = $"Тут должно быть на русском: {omdbResponse.Plot}"
                    },
                }
            },
            Poster = omdbResponse.Poster,
            IMDBRating = double.TryParse(omdbResponse.imdbRating, out var rating) ? rating : (double?)null,
            Employees = filmEmployees,
            Genres = genres,
            CountryId = 1,
            Duration = double.TryParse(durationString, out var duration) ? duration : 0
        };

        await _context.AddAsync(film);
        return Result.Success(film);
    }
}
