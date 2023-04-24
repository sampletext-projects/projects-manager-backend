using DataAccess.RepositoryNew;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class Registrar
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ProjectsContext>(
            options => options.UseNpgsql(
                configuration.GetConnectionString("Projects")
            )
        );

        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));

        return services;
    }
}