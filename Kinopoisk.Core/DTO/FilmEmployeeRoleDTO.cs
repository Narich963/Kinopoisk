using Kinopoisk.Core.Enitites;

namespace Kinopoisk.Core.DTO;

public class FilmEmployeeRoleDTO
{
    public int Role { get; set; }
    public bool IsDirector { get; set; }    

    public int FilmEmployeeId { get; set; }
    public FilmEmployeeDTO? FilmEmployee { get; set; }

    public int FilmId { get; set; }
}
