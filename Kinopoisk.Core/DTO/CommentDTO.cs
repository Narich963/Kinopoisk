using Kinopoisk.Core.Enitites;

namespace Kinopoisk.Core.DTO;

public class CommentDTO
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int FilmId { get; set; }

    public int UserId { get; set; }
    public UserDTO? User { get; set; }
}
