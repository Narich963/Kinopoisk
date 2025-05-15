using Kinopoisk.Core.Enitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kinopoisk.DataAccess;

public class KinopoiskContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<FilmEmployee> FilmEmployees { get; set; }
    public DbSet<FilmEmployeeRole> FilmEmployeeRoles { get; set; }
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

        builder.Entity<FilmEmployeeRole>()
            .HasKey(ar => new { ar.FilmEmployeeID, ar.FilmId });

        builder.Entity<Comment>()
            .HasKey(c => new { c.UserId, c.FilmId });

        builder.Entity<Rating>()
            .HasKey(r => new { r.UserId, r.FilmId });
    }
}
