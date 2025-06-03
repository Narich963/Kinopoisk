using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Filters;

namespace Kinopoisk.Services.Interfaces;

public interface IFilmService : IService<FilmDTO, FilmFilter>
{
    Task<Result<FilmDTO>> GetByIdAsync(int? id);
    Task<IEnumerable<FilmDTO>> GetAllAsync();
    Task<Result> UpdateFilmGenres(int? filmId, List<int> genreIds);
    Task<Result> UpdateFilmEmployees(int? filmId, int? directorId, List<int> actorIds);
    Task<Result<FilmDTO>> AddOrEditAsync(FilmDTO filmDto, bool? isNew);
}
