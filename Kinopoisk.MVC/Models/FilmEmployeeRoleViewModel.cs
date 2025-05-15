using Kinopoisk.Services.DTO;

namespace Kinopoisk.MVC.Models;

public class FilmEmployeeRoleViewModel
{
    public int Id { get; set; }
    public int Role { get; set; }

    public int FileEmployeeId { get; set; }
    public FilmEmployeeViewModel? FileEmployee { get; set; }

    public int FilmId { get; set; }
}
