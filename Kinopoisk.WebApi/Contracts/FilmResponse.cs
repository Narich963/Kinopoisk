namespace Kinopoisk.WebApi.Contracts;

public class FilmResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime PublishDate { get; set; }
    public CountryResponse? Country { get; set; }

    public string Duration { get; set; }
    public double? IMDBRating { get; set; }
    public double SitesRating { get; set; } = 0;

    public FilmEmployeeRoleResponse? Director { get; set; }

    public List<FilmGenreResponse> Genres { get; set; }
    public List<CommentResponse>? Comments { get; set; }
    public List<FilmEmployeeRoleResponse> Actors { get; set; }

    public FilmResponse()
    {
        Genres = new();
        Actors = new();
        Comments = new();
    }
}
