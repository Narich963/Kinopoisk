using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;

namespace Kinopoisk.DataAccess.Interfaces;

public interface IFilmRepository : IRepository<Film>
{
    IQueryable<Film> GetAllAsQueryable();
    Task<Result<Film>> GetByIdAsync(int id);
    Task<IEnumerable<Film>> GetAllAsync();
}
