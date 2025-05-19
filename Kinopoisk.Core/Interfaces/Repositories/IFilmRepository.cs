using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IFilmRepository : IRepository<Film>
{
    Task<PagedResult<Film>> GetPagedAsync(FilterModel<Film> filterModel);
    Task<Result<Film>> GetByIdAsync(int id);
}
