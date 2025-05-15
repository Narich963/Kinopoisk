using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.Core.Enitites;

public class FilmEmployeeRole
{
    public int Role { get; set; }
    public bool IsDirector { get; set; }

    public int FilmEmployeeID { get; set; }
    public FilmEmployee? FilmEmployee { get; set; }

    public int FilmId { get; set; }
    public Film? Film { get; set; }
}
