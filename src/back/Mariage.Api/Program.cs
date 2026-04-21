using Mariage.Api;
using Mariage.Api.Controllers;
using Mariage.Api.Errors;
using Mariage.Application;
using Mariage.Infrastructure;
using Mariage.Infrastructure.Persistence;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
var databaseConnectionString =
    builder.Configuration.GetConnectionString("postgresdb") ??
    builder.Configuration.GetConnectionString("MariageDatabase") ??
    throw new InvalidOperationException("A database connection string must be configured.");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod();

        if (allowedOrigins is { Length: > 0 })
        {
            policy.WithOrigins(allowedOrigins);
            return;
        }

        policy.AllowAnyOrigin();
    });
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsAdmin", policy => policy.RequireRole("Admin"));

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddDbContextPool<MariageDbContext>(options =>
        options.UseNpgsql(databaseConnectionString, npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()))
    .AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter("Login", opt =>
        {
            opt.Window = TimeSpan.FromSeconds(10);
            opt.PermitLimit = 3;
        });
    });

builder.AddServiceDefaults();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapDefaultEndpoints();
}

//Middleware
app.UseCors("CorsPolicy");

app.UseErrorHandling();
app.UseHttpsRedirection();
app.UseRouting();
app.UseRateLimiter(); //After UseRouting
app.UseStatusCodePages();
app.UseAuthentication();
app.UseAuthorization();

//Controllers
app.UseWakingUpController();
app.UseAuthenticationController();
app.UseWeddingListController();
app.UsePicturesController();
app.UseUserInfosController();
app.UseAccommodationController();

app.Run();