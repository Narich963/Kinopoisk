
using Kinopoisk.Core.DTO;

namespace Kinopoisk.MVC.Models;

public class CommentViewModel
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int FilmId { get; set; }

    public int UserId { get; set; }
    public UserDTO? User { get; set; }
}
