using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Enitites.Localization;
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
    public DbSet<Country> Countries { get; set; }
    public DbSet<FilmGenre> FilmGenres { get; set; }

    public DbSet<Localization> Localizations { get; set; }
    public DbSet<LocalizationSet> LocalizationSets { get; set; }
    public DbSet<Culture> Cultures { get; set; }

    public KinopoiskContext(DbContextOptions<KinopoiskContext> opts) : base(opts)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<FilmEmployeeRole>()
            .HasKey(ar => new { ar.FilmEmployeeID, ar.FilmId });

        builder.Entity<Rating>()
            .HasKey(r => new { r.UserId, r.FilmId });

        builder.Entity<FilmGenre>()
            .HasKey(fg => new { fg.FilmId, fg.GenreId });
    }
}
