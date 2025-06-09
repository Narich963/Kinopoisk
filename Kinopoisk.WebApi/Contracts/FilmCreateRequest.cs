namespace Kinopoisk.WebApi.Contracts;

public class FilmCreateRequest
{
    public string Name { get; set; } 
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public double Duration { get; set; }

    public int CountryId { get; set; }
    public int DirectorId { get; set; }
    public List<int> ActorIds { get; set; }
    public List<int> GenreIds { get; set; }
    public FilmCreateRequest()
    {
        ActorIds = new();
        GenreIds = new();
    }
}
