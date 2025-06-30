namespace Kinopoisk.Core.Enitites;

public class FilmGenre
{
    public int FilmId { get; set; }
    public virtual Film? Film { get; set; }

    public int GenreId { get; set; }
    public virtual Genre? Genre { get; set; }
}
