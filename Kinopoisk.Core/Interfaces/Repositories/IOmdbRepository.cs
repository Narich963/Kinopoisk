using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IOmdbRepository
{
    Task<Result<Film>> ImportFilm(string idOrTitle);
}
