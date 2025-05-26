namespace Kinopoisk.MVC.Models;

public class FilmEmployeeRoleViewModel
{
    public int Role { get; set; }
    public bool IsDirector { get; set; }

    public int FilmEmployeeId { get; set; }
    public FilmEmployeeViewModel? FilmEmployee { get; set; }

    public int FilmId { get; set; }

    public bool IsForDeleting { get; set; } = false;
}
