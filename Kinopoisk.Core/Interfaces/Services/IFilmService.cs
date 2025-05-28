using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.MVC.Models;

namespace Kinopoisk.Services.Interfaces;

public interface IFilmService : IService<FilmDTO, FilmFilter>
{
    Task<Result<FilmDTO>> GetByIdAsync(int? id);
    Task<IEnumerable<FilmDTO>> GetAllAsync();
    Task<Result> RemoveEmployeeFromFilm(int? filmId, int? employeeId);
    Task<Result> RemoveGenreFromFilm(int? filmId, int? genreId);
    Task<Result> AddGenreToFilm(int? filmId, int? genreId);
}
