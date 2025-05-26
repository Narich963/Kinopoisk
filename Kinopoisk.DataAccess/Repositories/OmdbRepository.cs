using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Kinopoisk.DataAccess.Repositories;

public class OmdbRepository : IOmdbRepository
{
    private readonly HttpClient _httpClient;
    private readonly KinopoiskContext _context;
    private readonly string apiKey = "2bafad4f";
    private readonly string apiUrl = "https://www.omdbapi.com/";

    public OmdbRepository(HttpClient httpClient, KinopoiskContext context)
    {
        _httpClient = httpClient;
        _context = context;
    }

    public async Task<Result<Film>> ImportFilm(string idOrTitle)
    {
        var urlById = $"{apiUrl}?i={Uri.EscapeDataString(idOrTitle)}&apikey={apiKey}";
        var urlByTitle = $"{apiUrl}?t={Uri.EscapeDataString(idOrTitle)}&apikey={apiKey}";

        var response = await _httpClient.GetAsync(urlById);

        var json = await response.Content.ReadAsStringAsync();
        var omdbResponse = JsonSerializer.Deserialize<OmdbResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (omdbResponse == null || string.Equals(omdbResponse.Response, "False", StringComparison.OrdinalIgnoreCase))
        {
            response = await _httpClient.GetAsync(urlByTitle);
            json = await response.Content.ReadAsStringAsync();

            omdbResponse = JsonSerializer.Deserialize<OmdbResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (omdbResponse == null || string.Equals(omdbResponse.Response, "False", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Failure<Film>("Failed to fetch data from OMDb API.");
            }
        }

        if (await _context.Films.AnyAsync(f => f.Name == omdbResponse.Title && f.PublishDate == Convert.ToDateTime(omdbResponse.Released)))       
            return Result.Failure<Film>("Film already exists in the database.");
        

        var genresResponse = omdbResponse.Genre?.Split(',').Select(g => g.Trim()).ToList();
        var genres = new List<FilmGenre>();
        if (genresResponse != null && genresResponse.Count > 0)
        {
            foreach (var genre in genresResponse)
            {
                if (!await _context.Genres.AnyAsync(g => g.Name == genre))
                {
                    await _context.Genres.AddAsync(new Genre { Name = genre});
                    await _context.SaveChangesAsync();
                }

                genres.Add(new FilmGenre
                {
                    GenreId = await _context.Genres.Where(g => g.Name == genre).Select(g => g.Id).FirstOrDefaultAsync(),
                });
            }
        }

        var actorsResponse = omdbResponse.Actors?.Split(',').Select(a => a.Trim()).ToList();
        var filmEmployees = new List<FilmEmployeeRole>();
        if (actorsResponse != null && actorsResponse.Count > 0)
        {
            foreach (var actor in actorsResponse)
            {
                if (!await _context.FilmEmployees.AnyAsync(fe => fe.Name == actor))
                {
                    await _context.FilmEmployees.AddAsync(new FilmEmployee { Name = actor });
                    await _context.SaveChangesAsync();
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
            await _context.SaveChangesAsync();
        }
        filmEmployees.Add(new FilmEmployeeRole
        {
            FilmEmployeeID = await _context.FilmEmployees.Where(f => f.Name == directorResponse).Select(f => f.Id).FirstOrDefaultAsync(),
            Role = 1,
            IsDirector = true
        });

        var durationString = omdbResponse.Runtime.Replace(" min", "").Trim();

        var film = new Film
        {
            Name = omdbResponse.Title,
            PublishDate = Convert.ToDateTime(omdbResponse.Released),
            Description = omdbResponse.Plot,
            Poster = omdbResponse.Poster,
            IMDBRating = double.TryParse(omdbResponse.imdbRating, out var rating) ? rating : (double?)null,
            Employees = filmEmployees,
            Genres = genres,
            CountryId = 1,
            Duration = double.TryParse(durationString, out var duration) ? duration : 0
        };

        await _context.AddAsync(film);
        await _context.SaveChangesAsync();
        return Result.Success(film);
    }
}
