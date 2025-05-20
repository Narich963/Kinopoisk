using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.MVC.Models;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IFilmRepository : IRepository<Film, FilmFilter>
{
    Task<Result<Film>> GetByIdAsync(int id);
}
