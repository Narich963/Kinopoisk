using Kinopoisk.Core.Enitites;
using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.MVC.Models;

public class FilmsViewModel
{
    public int Id { get; set; }
    public string? Poster { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime PublishDate { get; set; }
    public int CountryId { get; set; }
    public CountryViewModel? Country { get; set; }

    [RegularExpression(@"^\d{1,2}h\s\d{1,2}min$", ErrorMessage = "Format must be like '2h 30min'")]
    public string Duration { get; set; }
    public double? IMDBRating { get; set; }
    public double SitesRating { get; set; } = 0;

    public FilmEmployeeRoleViewModel? Director { get; set; }

    public List<FilmGenreViewModel> Genres { get; set; }
    public List<CommentViewModel>? Comments { get; set; }
    public List<FilmEmployeeRoleViewModel> Actors { get; set; }

    public bool? IsNew { get; set; }
    public List<int> SelectedGenreIds { get; set; } = new();
    public FilmsViewModel()
    {
        Genres = new();
        Actors = new();
        Comments = new();
    }
}
