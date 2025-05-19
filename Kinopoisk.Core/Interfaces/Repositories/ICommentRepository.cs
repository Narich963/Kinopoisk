using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    Task<Result<IEnumerable<Comment>>> GetAllByFilmAsync(int filmId);
}
