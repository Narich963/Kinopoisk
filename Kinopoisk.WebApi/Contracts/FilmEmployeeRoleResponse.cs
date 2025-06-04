namespace Kinopoisk.WebApi.Contracts;

public class FilmEmployeeRoleResponse
{
    public int Role { get; set; }
    public bool IsDirector { get; set; }

    public int FilmEmployeeId { get; set; }
    public FilmEmployeeResponse? FilmEmployee { get; set; }

    public int FilmId { get; set; }
}
