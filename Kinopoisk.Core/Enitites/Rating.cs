namespace Kinopoisk.Core.Enitites;

public class Rating
{
    public double Value { get; set; }
    
    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public int FilmId { get; set; }
    public virtual Film? Film { get; set; }
}
