using Kinopoisk.Core.Enitites;

namespace Kinopoisk.MVC.Models;

public class FilmsViewModel
{
    public int Id { get; set; }
    public string? Poster { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime PublishDate { get; set; }
    public string Country { get; set; }
    public string Duration { get; set; }
    public double? IMDBRating { get; set; }
    public double? UsersRating { get; set; }

    public string DirectorName { get; set; }

    public List<string> Genres { get; set; }
    public List<CommentViewModel> Comments { get; set; }
    //public List<Rating> Ratings { get; set; }
    public List<string> Actors { get; set; }
}
