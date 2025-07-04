using Kinopoisk.Core.Enitites.Localization;
using Kinopoisk.Core.Enums;

namespace Kinopoisk.Core.Enitites;

public class Film : LocalizationSet
{
    public string? Poster { get; set; }
    public DateTime? PublishDate { get; set; }
    public double Duration { get; set; }
    public double? IMDBRating { get; set; }
    public double SitesRating { get; set; } = 0;

    public int? CountryId { get; set; }
    public virtual Country? Country { get; set; }
    public virtual List<FilmGenre> Genres { get; set; }
    public virtual List<Comment> Comments { get; set; }
    public virtual List<Rating> Ratings { get; set; }
    public virtual List<FilmEmployeeRole> Employees { get; set; }

    public Film()
    {
        Genres = new();
        Comments = new();
        Ratings = new();
        Employees = new();
    }
}
