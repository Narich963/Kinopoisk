namespace Kinopoisk.WebApi.Contracts;

public class FilmCreateRequest
{
    public string Name { get; set; } 
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public double Duration { get; set; }
    public string Country { get; set; }
}
