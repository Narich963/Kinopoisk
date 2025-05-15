using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;

namespace Kinopoisk.Services.Interfaces;

public interface ICommentService 
{
    Task<Result<IEnumerable<CommentDTO>>> GetAllByFilmAsync(int? filmId);
    Task<Result<CommentDTO>> GetByIdAsync(int? id);
    Task<IEnumerable<CommentDTO>> GetAllAsync();
}
