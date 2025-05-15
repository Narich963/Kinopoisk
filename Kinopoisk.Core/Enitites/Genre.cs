using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kinopoisk.Core.Enitites;

public class Genre
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    public List<Film> Films { get; set; }
    public Genre()
    {
        Films = new();
    }
}
