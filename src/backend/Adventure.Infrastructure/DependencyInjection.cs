using Adventure.Application.Interfaces;
using Adventure.Domain.Interfaces;
using Adventure.Infrastructure.AI;
using Adventure.Infrastructure.Persistence;
using Adventure.Infrastructure.Persistence.Repositories;
using Adventure.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adventure.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Data Source=adventure.db";

        services.AddDbContext<AdventureDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddSingleton<IDiceService, DiceService>();
        services.AddSingleton<ICombatAI, PlaceholderCombatAI>();

        return services;
    }
}
