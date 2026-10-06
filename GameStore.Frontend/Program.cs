using System.IdentityModel.Tokens.Jwt;
using GameStore.Frontend.Authorization;
using GameStore.Frontend.Clients;
using GameStore.Frontend.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();

var gameStoreApiUrl = builder.Configuration["GameStoreApiUrl"] ??
    throw new Exception("GameStoreApiUrl is not set");

builder.Services.AddHttpContextAccessor()
                .AddTransient<AuthorizationHandler>();

builder.Services.AddHttpClient<GamesClient>(
    client => client.BaseAddress = new Uri(gameStoreApiUrl))
    .AddHttpMessageHandler<AuthorizationHandler>();

builder.Services.AddHttpClient<GenresClient>(
    client => client.BaseAddress = new Uri(gameStoreApiUrl));

builder.Services
       .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
       .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
       {
           options.Authority = "https://login.microsoftonline.com/60ca49b1-bfa4-4aa0-b737-3f1a6f7346e4/v2.0";
           options.ClientId = "3f652580-bf09-4bae-bd1e-143bad632592";
           options.Scope.Add("api://c7805272-1f54-44c7-a36d-5b9100e1dd8a/gamestore_api_all");
           options.ResponseType = OpenIdConnectResponseType.Code;
           options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           options.SaveTokens = true;
           options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
       })
       .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

builder.Services.AddAuthorizationBuilder();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>();
app.MapLoginAndLogout();

app.Run();