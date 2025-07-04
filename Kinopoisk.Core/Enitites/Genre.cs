using Kinopoisk.Core.Enitites.Localization;
using Kinopoisk.Core.Enums;

namespace Kinopoisk.Core.Enitites;

public class Genre : LocalizationSet
{
    public virtual List<FilmGenre> Films { get; set; }
    public Genre()
    {
        Films = new();
    }
}
