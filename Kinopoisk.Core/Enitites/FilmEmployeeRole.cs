﻿namespace Kinopoisk.Core.Enitites;

public class FilmEmployeeRole
{
    public int Role { get; set; }
    public bool IsDirector { get; set; }

    public int FilmEmployeeID { get; set; }
    public virtual FilmEmployee? FilmEmployee { get; set; }

    public int FilmId { get; set; }
    public virtual Film? Film { get; set; }
}
