using Kinopoisk.Core.Enitites;

namespace Kinopoisk.Core.DTO;

public class RatingDTO
{
    public double Value { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int FilmId { get; set; }
}
