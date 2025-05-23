namespace Kinopoisk.MVC.Models;

public class FilmGenreViewModel
{
    public int FilmId { get; set; }

    public int GenreId { get; set; }
    public GenreViewModel Genre { get; set; }
}
