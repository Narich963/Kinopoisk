using Kinopoisk.Core.Enitites.Localization;

namespace Kinopoisk.Core.Enitites;

public class FilmEmployee
{
    public int Id { get; set; }

    public int NameId { get; set; }
    public virtual LocalizationSet Name { get; set; }

    public virtual List<FilmEmployeeRole> ActorRoles { get; set; }

    public FilmEmployee()
    {
        ActorRoles = new();
    }
}
