namespace Kinopoisk.Core.Enitites;

public class Film
{
    public int Id { get; set; }
    public string? Poster { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int PublishYear { get; set; }
    public string Country { get; set; }
    public double Duration { get; set; }
    public double? IMDBRating { get; set; }
    public double? UsersRating { get; set; }

    public int DirectorId { get; set; }
    public Director? Director { get; set; }

    public List<Actor> Actors { get; set; }
    public List<Genre> Genres { get; set; }
    public List<Comment> Comments { get; set; }
    public List<Rating> Ratings { get; set; }
    public List<ActorRole> ActorRoles { get; set; }

    public Film()
    {
        Genres = new();
        Actors = new();
        Comments = new();
        Ratings = new();
        ActorRoles = new();
    }
}
