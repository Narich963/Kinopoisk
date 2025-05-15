using Kinopoisk.Core.Enitites;

namespace Kinopoisk.Services.DTO;

public class ActorRoleDTO
{
    public int Id { get; set; }
    public int Role { get; set; }

    public int ActorId { get; set; }
    public ActorDTO? Actor { get; set; }

    public int FilmId { get; set; }
    //public FilmDTO? Film { get; set; }
}
