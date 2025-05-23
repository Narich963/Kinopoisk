﻿using Kinopoisk.Core.Enitites;

namespace Kinopoisk.WebApi.Contracts;

public class FilmResponse
{
    public int Id { get; set; }
    public string? Poster { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime PublishDate { get; set; }
    public string Country { get; set; }
    public double Duration { get; set; }
    public double? IMDBRating { get; set; }
    public double? UsersRating { get; set; }

    public string DirectorName { get; set; }

    public List<string> Genres { get; set; }
    //public List<Comment> Comments { get; set; }
    //public List<Rating> Ratings { get; set; }
    public List<string> ActorRoles { get; set; }
}
