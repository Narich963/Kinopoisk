using Kinopoisk.Core.Enitites.Localization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kinopoisk.Core.Enitites;

public class Genre
{
    public int Id { get; set; }

    public int NameId { get; set; }
    public virtual LocalizationSet Name { get; set; }

    public virtual List<FilmGenre> Films { get; set; }
    public Genre()
    {
        Films = new();
    }
}
