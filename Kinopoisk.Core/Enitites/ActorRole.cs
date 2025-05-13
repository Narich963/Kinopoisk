using Kinopoisk.Core.Enums;

namespace Kinopoisk.Core.Enitites;

public class ActorRole
{
    public int Id { get; set; }
    public FilmRole Role { get; set; }

    public int ActorId { get; set; }
    public Actor? Actor { get; set; }

    public int FilmId { get; set; }
    public Film? Film { get; set; }
}
