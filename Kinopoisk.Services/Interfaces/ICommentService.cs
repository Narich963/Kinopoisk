using CSharpFunctionalExtensions;
using Kinopoisk.Services.DTO;

namespace Kinopoisk.Services.Interfaces;

public interface ICommentService : IService<CommentDTO>
{
    Task<Result<IEnumerable<CommentDTO>>> GetAllByFilmAsync(int? filmId);
}
