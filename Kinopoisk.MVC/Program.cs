using Kinopoisk.Core.Enitites;
using Kinopoisk.DataAccess;
using Kinopoisk.DataAccess.DataSeeding;
using Kinopoisk.DataAccess.Interfaces;
using Kinopoisk.DataAccess.Repositories;
using Kinopoisk.MVC.Initializers;
using Kinopoisk.Services.Interfaces;
using Kinopoisk.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddTransient<IRepository<Film>, FilmsRepository>();
builder.Services.AddTransient<IFilmsService, FilmsService>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
