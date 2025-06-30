using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.Core.Enitites;

public class Comment
{
    public int Id { get; set; }

    [MaxLength(250)]
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int FilmId { get; set; }
    public virtual Film? Film { get; set; }

    public int UserId { get; set; }
    public virtual User? User { get; set; }
}
