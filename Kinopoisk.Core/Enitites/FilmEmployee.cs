using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.Core.Enitites;

public class FilmEmployee
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    public virtual List<FilmEmployeeRole> ActorRoles { get; set; }

    public FilmEmployee()
    {
        ActorRoles = new();
    }
}
