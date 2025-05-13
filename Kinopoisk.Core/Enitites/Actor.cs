namespace Kinopoisk.Core.Enitites;

public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<ActorRole> ActorRoles { get; set; }

    public Actor()
    {
        ActorRoles = new();
    }
}
