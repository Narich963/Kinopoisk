using Kinopoisk.Core.DTO;

namespace Kinopoisk.WebApi.Contracts;

public class CommentResponse
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int FilmId { get; set; }

    public int UserId { get; set; }
}
