namespace Kinopoisk.Core.DTO;

public class FilmGenreDTO
{
    public int FilmId { get; set; }

    public int GenreId { get; set; }
    public GenreDTO? Genre { get; set; }
}
