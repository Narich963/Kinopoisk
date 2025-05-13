namespace Kinopoisk.Core.Enitites;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Film> Films { get; set; }
    public Genre()
    {
        Films = new();
    }
}
