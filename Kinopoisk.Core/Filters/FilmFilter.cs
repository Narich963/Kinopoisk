using Kinopoisk.MVC.Models;

namespace Kinopoisk.Core.Filters;

public class FilmFilter : DataTablesRequestModel
{
    public string? Name { get; set; }
    public string? Year { get; set; }
    public string? Country { get; set; }
    public string? Actor { get; set; }
    public string? Director { get; set; }
}
