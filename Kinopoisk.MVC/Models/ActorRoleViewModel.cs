using Kinopoisk.Services.DTO;

namespace Kinopoisk.MVC.Models;

public class ActorRoleViewModel
{
    public int Id { get; set; }
    public int Role { get; set; }

    public int ActorId { get; set; }
    public ActorViewModel? Actor { get; set; }

    public int FilmId { get; set; }
}
