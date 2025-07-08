using AutoMapper.Extensions.ExpressionMapping;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.DataAccess;
using Kinopoisk.DataAccess.Repositories;
using Kinopoisk.MVC.Hubs;
using Kinopoisk.MVC.Initializers;
using Kinopoisk.Services.Interfaces;
using Kinopoisk.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using Serilog;
using System.Globalization;
using Kinopoisk.DataAccess.DataSeeding;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogger();
ConfigurePages();
ConfigureDatabase();
ConfigureServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DataSeeder.SeedAsync(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

ConfigureLocalization();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages();

app.MapGet("/", () =>
{
    return Results.Redirect("/Home/Index");
});
app.MapHub<CommentHub>("/comments");

app.Run();

void ConfigureLogger()
{
    Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .Enrich.FromLogContext()
    .CreateLogger();

    builder.Host.UseSerilog();
}
void ConfigurePages()
{
    builder.Services.AddLocalization(opts => opts.ResourcesPath = "Resources");
    builder.Services.AddRazorPages();
    builder.Services
        .AddControllersWithViews()
        .AddViewLocalization();
}
void ConfigureDatabase()
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<KinopoiskContext>(opts => opts.UseLazyLoadingProxies().UseSqlServer(connectionString, m => m.MigrationsAssembly("Kinopoisk.DataAccess")))
        .AddIdentity<User, IdentityRole<int>>(opts =>
        {
            opts.Password.RequireDigit = false;
            opts.Password.RequiredLength = 5;
            opts.Password.RequireNonAlphanumeric = false;
            opts.Password.RequireUppercase = false;
            opts.Password.RequireLowercase = false;
        })
        .AddEntityFrameworkStores<KinopoiskContext>();
}
void ConfigureServices()
{
    builder.Services.AddHttpClient();
    builder.Services.AddMemoryCache();
    builder.Services.AddSignalR();
    builder.Services.AddAutoMapper(opts =>
    {
        opts.AddExpressionMapping();
    }, typeof(MapperInitializer));

    builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
    builder.Services.AddTransient<IFilmRepository, FilmRepository>();
    builder.Services.AddTransient<IFilmService, FilmService>();
    builder.Services.AddTransient<ICommentRepository, CommentRepository>();
    builder.Services.AddTransient<ICommentService, CommentService>();
    builder.Services.AddTransient<IUserService, UserService>();
    builder.Services.AddTransient<IRatingRepository, RatingRepository>();
    builder.Services.AddTransient<IRatingService, RatingService>();
    builder.Services.AddTransient<IGenreService, GenreService>();
    builder.Services.AddTransient<ICountryService, CountryService>();
    builder.Services.AddTransient<IFilmEmployeeService, FilmEmployeeService>();
    builder.Services.AddTransient<IOmdbService, OmdbService>();
    builder.Services.AddTransient<IOmdbRepository, OmdbRepository>();
    builder.Services.AddTransient<IDocumentService, DocumentService>();
    builder.Services.AddTransient<ILocalizationRepository, LocalizationRepository>();
    builder.Services.AddTransient<ILocalizationService, LocalizationService>();

    QuestPDF.Settings.License = LicenseType.Community;
}
void ConfigureLocalization()
{
    var supportedCultures = new List<CultureInfo>
    {
        new("en"),
        new("ru")
    };
    var options = new RequestLocalizationOptions
    {
        DefaultRequestCulture = new("en"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures,
        RequestCultureProviders =
        {
            new CookieRequestCultureProvider(),
            new QueryStringRequestCultureProvider()
        }
    };
    app.UseRequestLocalization(options);
}