namespace Kinopoisk.Core.Enitites;

public class Rating
{
    public int Value { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }

    public int FilmId { get; set; }
    public Film? Film { get; set; }
}
