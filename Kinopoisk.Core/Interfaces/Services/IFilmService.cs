using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;

namespace Kinopoisk.Services.Interfaces;

public interface IFilmService : IService<FilmDTO>
{
    Task<PagedResult<FilmDTO>> GetPagedAsync(FilterModel<FilmDTO> filterModel);
    Task<Result<FilmDTO>> GetByIdAsync(int? id);
    Task<IEnumerable<FilmDTO>> GetAllAsync();
}
