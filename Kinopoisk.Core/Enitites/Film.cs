using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.Core.Enitites;

public class Film
{
    public int Id { get; set; }
    public string? Poster { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string? Description { get; set; }
    public DateTime PublishDate { get; set; }
    public string Country { get; set; }
    public double Duration { get; set; }
    public double? IMDBRating { get; set; }
    public double? UsersRating { get; set; }

    public List<Genre> Genres { get; set; }
    public List<Comment> Comments { get; set; }
    public List<Rating> Ratings { get; set; }
    public List<FilmEmployeeRole> Employees { get; set; }

    public Film()
    {
        Genres = new();
        Comments = new();
        Ratings = new();
        Employees = new();
    }
}
