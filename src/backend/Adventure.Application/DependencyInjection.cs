using System.Reflection;
using Adventure.Application.Behaviors;
using Adventure.Application.Features.Combat;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Adventure.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(assembly);

        services.AddSingleton<ICombatEncounterStore, CombatEncounterStore>();
        services.AddScoped<CombatTurnResolver>();

        return services;
    }
}
