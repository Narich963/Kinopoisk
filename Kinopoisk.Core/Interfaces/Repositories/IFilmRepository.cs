using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IFilmRepository : IRepository<Film, FilmFilter>
{
    Task<Result<Film>> GetByIdAsync(int id);
    Task<Result<Film>> AddAsync(Film entity);
    Task<Result> UpdateGenres(int filmId, List<int> genreIds);
    Task<Result> UpdateFilmEmployees(int filmId, int directorId, List<int> actorIds);
}
