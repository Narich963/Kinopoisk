using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.MVC.Models;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IFilmRepository : IRepository<Film>
{
    Task<DataTablesResult<Film>> GetPagedAsync(FilmFilter model);
    Task<Result<Film>> GetByIdAsync(int id);
}
