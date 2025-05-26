using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.MVC.Models;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IFilmRepository : IRepository<Film, FilmFilter>
{
    Task<Result<Film>> GetByIdAsync(int id);
    Task<Result> RemoveEmployeeFromFilm(int filmId, int employeeId);
    Task<Result> RemoveGenreFromFilm(int filmId, int genreId);
}
