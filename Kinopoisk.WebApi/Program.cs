using Duende.IdentityServer.Models;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.DataAccess;
using Kinopoisk.DataAccess.Repositories;
using Kinopoisk.Services.Interfaces;
using Kinopoisk.Services.Services;
using Kinopoisk.WebApi.IdentityServer;
using Kinopoisk.WebApi.Initializers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Kinopoisk Web API",
        Version = "v1",
        Description = "API for Kinopoisk application"
    });

    opts.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("https://localhost:5001/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "kinopoisk_api", "Access to Kinopoisk API" }
                }
            }
        }
    });

    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            ["kinopoisk_api"]
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.RequireHttpsMetadata = true;
        opts.SaveToken = true;
        opts.Authority = "https://localhost:5001";
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidIssuer = "https://localhost:5001",
        };

        opts.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();


builder.Services.AddEndpointsApiExplorer();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<KinopoiskContext>(opts => opts.UseSqlServer(connectionString, m => m.MigrationsAssembly("Kinopoisk.DataAccess")))
    .AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<KinopoiskContext>();

var identityServerClient = builder.Configuration
    .GetSection("IdentityServer:Client")
    .Get<IdentityServerClient>();
var identityServerScope = builder.Configuration
    .GetSection("IdentityServer:ApiScope")
    .Get<IdentityServerScope>();

builder.Services.AddIdentityServer()
    .AddInMemoryClients([new Client
        {
            ClientId = identityServerClient.ClientId,
            ClientSecrets = { new Secret(identityServerClient.ClientSecret.Sha256()) },
            AllowedGrantTypes = identityServerClient.AllowedGrantTypes,
            AllowedScopes = identityServerClient.AllowedScopes,
        }
    ])
    .AddInMemoryApiScopes([new ApiScope
        {
            Name = identityServerScope.Name,
            DisplayName = identityServerScope.DisplayName
        }
    ])
    .AddAspNetIdentity<User>()
    .AddDeveloperSigningCredential();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IFilmRepository, FilmRepository>();
builder.Services.AddTransient<IFilmService, FilmService>();
builder.Services.AddTransient<IFilmEmployeeService, FilmEmployeeService>();
builder.Services.AddTransient<IGenreService, GenreService>();
builder.Services.AddAutoMapper(typeof(MapperInitializer));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts => 
    {
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Kinopoisk.WebApi v1");

        opts.OAuthClientId(identityServerClient.ClientId);
        opts.OAuthClientSecret(identityServerClient.ClientSecret);
        opts.OAuthAppName("Kinopoisk Web API");
        opts.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
