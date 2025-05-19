namespace Kinopoisk.MVC.Models;

public class FilmFilterModel
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? SortField { get; set; }
    public bool IsAscending { get; set; } = true;

    public string? Name { get; set; }
    public int? Year { get; set; }
    public string? Country { get; set; }
    public string? ActorName { get; set; }
    public string? Director { get; set; }
}
