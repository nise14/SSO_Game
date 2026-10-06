using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

builder.Services
       .AddAuthentication()
       .AddJwtBearer(options =>
       {
            options.Authority = "https://login.microsoftonline.com/60ca49b1-bfa4-4aa0-b737-3f1a6f7346e4/v2.0";
            options.Audience = "60ca49b1-bfa4-4aa0-b737-3f1a6f7346e4";
       });

builder.Services.AddAuthorizationBuilder();

var app = builder.Build();

app.MapGamesEndpoints();
app.MapGenresEndpoints();

await app.MigrateDbAsync();

app.Run();
