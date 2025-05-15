using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.Core.Enitites;

public class Actor
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    public List<ActorRole> ActorRoles { get; set; }

    public Actor()
    {
        ActorRoles = new();
    }
}
