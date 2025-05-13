using Kinopoisk.Core.Enitites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kinopoisk.DataAccess;

public class KinopoiskContext : IdentityDbContext
{
    public DbSet<Actor> Actors { get; set; }
    public DbSet<ActorRole> ActorRoles { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<User> Users { get; set; }

    public KinopoiskContext(DbContextOptions opts) : base(opts)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
