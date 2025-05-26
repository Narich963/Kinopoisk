using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;

namespace Kinopoisk.Core.Interfaces.Services;

public interface IOmdbService
{
    Task<Result<FilmDTO>> ImportFilm(string idOrTitle);
}
