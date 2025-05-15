using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;

namespace Kinopoisk.Services.Interfaces;

public interface ICommentService : IService<CommentDTO>
{
    Task<Result<IEnumerable<CommentDTO>>> GetAllByFilmAsync(int? filmId);
}
