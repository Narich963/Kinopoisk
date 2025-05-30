using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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
                if (!await _context.Genres.AnyAsync(g => g.Name == genre))
                {
                    await _context.Genres.AddAsync(new Genre { Name = genre});
                }

                genres.Add(new FilmGenre
                {
                    GenreId = await _context.Genres.Where(g => g.Name == genre).Select(g => g.Id).FirstOrDefaultAsync(),
                });
            }
        }
        // It's neccessary to save changes here to ensure that genres are added before creating the film.
        await _context.SaveChangesAsync();

        var actorsResponse = omdbResponse.Actors?.Split(',').Select(a => a.Trim()).ToList();
        var filmEmployees = new List<FilmEmployeeRole>();
        if (actorsResponse != null && actorsResponse.Count > 0)
        {
            foreach (var actor in actorsResponse)
            {
                if (!await _context.FilmEmployees.AnyAsync(fe => fe.Name == actor))
                {
                    await _context.FilmEmployees.AddAsync(new FilmEmployee { Name = actor });
                }

                filmEmployees.Add(new FilmEmployeeRole
                {
                    FilmEmployeeID = await _context.FilmEmployees.Where(f => f.Name == actor).Select(f => f.Id).FirstOrDefaultAsync(),
                    Role = 1,
                    IsDirector = false
                });
            }
        }

        var directorResponse = omdbResponse.Director?.Trim();
        if (!await _context.FilmEmployees.AnyAsync(f => f.Name == directorResponse))
        {
            await _context.FilmEmployees.AddAsync(new FilmEmployee { Name = directorResponse });
        }
        filmEmployees.Add(new FilmEmployeeRole
        {
            FilmEmployeeID = await _context.FilmEmployees.Where(f => f.Name == directorResponse).Select(f => f.Id).FirstOrDefaultAsync(),
            Role = 1,
            IsDirector = true
        });
        // It's neccessary to save changes here to ensure that film employees are added before creating the film.
        await _context.SaveChangesAsync();


        var durationString = omdbResponse.Runtime.Replace(" min", "").Trim();
        var film = new Film
        {
            Name = omdbResponse.Title,
            PublishDate = DateTime.TryParse(omdbResponse.Released, out var date) ? date : null,
            Description = omdbResponse.Plot,
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
