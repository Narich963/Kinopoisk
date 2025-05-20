using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Filters;

namespace Kinopoisk.Services.Interfaces;

public interface ICommentService : IService<CommentDTO>
{
    Task<Result<DataTablesResult<CommentDTO>>> GetAllByFilmAsync(CommentFilter filter);
    Task<Result<CommentDTO>> GetByIdAsync(int? id);
    Task<IEnumerable<CommentDTO>> GetAllAsync();
}
