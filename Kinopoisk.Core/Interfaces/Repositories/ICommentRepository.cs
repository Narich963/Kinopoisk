using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    Task<DataTablesResult<Comment>> GetAllByFilmAsync(CommentFilter filter);
}
