using Kinopoisk.Core.Enitites;

namespace Kinopoisk.Services.DTO;

public class FilmDTO
{
    public int Id { get; set; }
    public string? Poster { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime PublishDate { get; set; }
    public double Duration { get; set; }
    public double? IMDBRating { get; set; }
    public double? UsersRating { get; set; }
    public int CountryId { get; set; }

    public CountryDTO Country { get; set; }
    public List<GenreDTO> Genres { get; set; }
    public List<CommentDTO> Comments { get; set; }
    //public List<RatingDTO> Ratings { get; set; }
    public List<FilmEmployeeRoleDTO> Employees { get; set; }
}
