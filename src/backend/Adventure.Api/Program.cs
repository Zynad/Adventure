using Adventure.Application;
using Adventure.Api.Middleware;
using Adventure.Infrastructure;
using Adventure.Infrastructure.Persistence;
using Adventure.Infrastructure.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Ensure database is created and seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AdventureDbContext>();
    await db.Database.MigrateAsync();

    var dataPath = Path.Combine(app.Environment.ContentRootPath, "..", "..", "..", "assets", "data");
    if (Directory.Exists(dataPath))
    {
        await DatabaseSeeder.SeedAsync(db, dataPath);
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors();
app.MapControllers();

app.Run();
