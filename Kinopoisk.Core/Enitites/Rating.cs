﻿namespace Kinopoisk.Core.Enitites;

public class Rating
{
    public double Value { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }

    public int FilmId { get; set; }
    public Film? Film { get; set; }
}
