using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IOmdbRepository
{
    Task<Result<Film>> ImportFilm(OmdbResponse omdbResponse);
}
