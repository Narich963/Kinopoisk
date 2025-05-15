using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.Core.Enitites;

public class ActorRole
{
    public int Role { get; set; }

    public int ActorId { get; set; }
    public Actor? Actor { get; set; }

    public int FilmId { get; set; }
    public Film? Film { get; set; }
}
