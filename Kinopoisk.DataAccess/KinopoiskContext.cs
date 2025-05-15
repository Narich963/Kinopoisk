using Kinopoisk.Core.Enitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kinopoisk.DataAccess;

public class KinopoiskContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<Actor> Actors { get; set; }
    public DbSet<ActorRole> ActorRoles { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<User> Users { get; set; }

    public KinopoiskContext(DbContextOptions<KinopoiskContext> opts) : base(opts)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ActorRole>()
            .HasKey(ar => new { ar.ActorId, ar.FilmId });

        builder.Entity<Comment>()
            .HasKey(c => new { c.UserId, c.FilmId });
    }
}
