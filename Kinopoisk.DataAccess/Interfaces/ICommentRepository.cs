using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;

namespace Kinopoisk.DataAccess.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<Result<IEnumerable<Comment>>> GetAllByFilmAsync(int filmId);
    Task<Result<Comment>> GetByIdAsync(int id);
    Task<IEnumerable<Comment>> GetAllAsync();
}
