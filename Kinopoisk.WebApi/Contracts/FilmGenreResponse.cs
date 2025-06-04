namespace Kinopoisk.WebApi.Contracts;

public class FilmGenreResponse
{
    public int FilmId { get; set; }

    public int GenreId { get; set; }
    public GenreResponse? Genre { get; set; }
}
