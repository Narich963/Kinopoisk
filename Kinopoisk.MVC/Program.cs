using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.DataAccess;
using Kinopoisk.DataAccess.DataSeeding;
using Kinopoisk.DataAccess.Repositories;
using Kinopoisk.MVC.Initializers;
using Kinopoisk.Services.Interfaces;
using Kinopoisk.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<KinopoiskContext>(opts => opts.UseSqlServer(connectionString, m => m.MigrationsAssembly("Kinopoisk.DataAccess")))
    .AddIdentity<User, IdentityRole<int>>(opts =>
    {
        opts.Password.RequireDigit = false;
        opts.Password.RequiredLength = 5;
        opts.Password.RequireNonAlphanumeric = false;
        opts.Password.RequireUppercase = false;
        opts.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<KinopoiskContext>();

builder.Services.AddAutoMapper(typeof(MapperInitializer));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IFilmRepository, FilmRepository>();
builder.Services.AddTransient<IFilmService, FilmService>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRatingRepository, RatingRepository>();
builder.Services.AddTransient<IRatingService, RatingService>();

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

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages();

app.MapGet("/", () =>
{
    return Results.Redirect("/Home/Index");
});

app.Run();
