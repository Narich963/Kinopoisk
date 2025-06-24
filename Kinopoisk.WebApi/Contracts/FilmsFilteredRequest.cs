namespace Kinopoisk.WebApi.Contracts;

public class FilmsFilteredRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public string? Column { get; set; } = "id";
    public string? Direction { get; set; } = "asc";
    public string? Search { get; set; }

    public string? Name { get; set; }
    public string? Year { get; set; }
    public string? Country { get; set; }
    public string? Actor { get; set; }
    public string? Director { get; set; }
}
