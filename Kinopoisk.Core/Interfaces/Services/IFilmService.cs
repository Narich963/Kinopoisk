using Kinopoisk.Core.DTO;

namespace Kinopoisk.Services.Interfaces;

public interface IFilmService : IService<FilmDTO>
{
    Task<IEnumerable<FilmDTO>> GetFilteredAsync(FilmsFilterDTO dto);
}
