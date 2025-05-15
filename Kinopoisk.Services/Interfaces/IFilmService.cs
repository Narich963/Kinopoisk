using Kinopoisk.Core.Enitites;
using Kinopoisk.Services.DTO;

namespace Kinopoisk.Services.Interfaces;

public interface IFilmService : IService<FilmDTO>
{
    Task<IEnumerable<FilmDTO>> GetFilteredAsync(string? name, int? year, string? country, string? actorName,
        string? director);
}
