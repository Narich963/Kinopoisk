using Kinopoisk.Core.Enitites;
using Kinopoisk.Services.DTO;

namespace Kinopoisk.WebApi.Contracts;

public class FilmResponse
{
    public int Id { get; set; }
    public string? Poster { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime PublishDate { get; set; }
    public string Country { get; set; }
    public double Duration { get; set; }
    public double? IMDBRating { get; set; }
    public double? UsersRating { get; set; }

    public int DirectorId { get; set; }
    public DirectorDTO? Director { get; set; }

    public List<ActorDTO> Actors { get; set; }
    public List<GenreDTO> Genres { get; set; }
    //public List<Comment> Comments { get; set; }
    //public List<Rating> Ratings { get; set; }
    public List<ActorRoleDTO> ActorRoles { get; set; }
}
