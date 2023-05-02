using BusinessLogic.Jwt;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic;

public static class Registrar
{
    public static IServiceCollection AddBLL(this IServiceCollection services, ConfigurationManager configuration)
    {
        var assembly = typeof(Registrar).Assembly;

        services.AddMediatR(x => x.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddTransient<IJwtGeneratorService, JwtGeneratorService>();
        
        return services;
    }
}