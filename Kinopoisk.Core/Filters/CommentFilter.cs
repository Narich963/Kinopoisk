using Kinopoisk.MVC.Models;

namespace Kinopoisk.Core.Filters;

public class CommentFilter : DataTablesRequestModel
{
    public int? FilmId { get; set; }
    public string? Username { get; set; }
    public string? Text { get; set; }
    public DateTime? CreatedAt { get; set; }
}
