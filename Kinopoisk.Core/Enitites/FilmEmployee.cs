using Kinopoisk.Core.Enitites.Localization;
using Kinopoisk.Core.Enums;

namespace Kinopoisk.Core.Enitites;

public class FilmEmployee : LocalizationSet
{
    public virtual List<FilmEmployeeRole> ActorRoles { get; set; }

    public FilmEmployee()
    {
        ActorRoles = new();
    }
}
