using Kinopoisk.Core.Enitites;
using Kinopoisk.DataAccess;
using Kinopoisk.DataAccess.Interfaces;
using Kinopoisk.DataAccess.Repositories;
using Kinopoisk.Services.Interfaces;
using Kinopoisk.Services.Services;
using Kinopoisk.WebApi.Initializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<KinopoiskContext>(opts => opts.UseSqlServer(connectionString, m => m.MigrationsAssembly("Kinopoisk.DataAccess")))
    .AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<KinopoiskContext>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IRepository<Film>, FilmsRepository>();
builder.Services.AddAutoMapper(typeof(MapperInitializer));
builder.Services.AddTransient<IFilmsService, FilmsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kinopoisk.WebApi v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
