
using BibliotecaApi.Infrastructure.Data;
using BibliotecaApi.Infrastructure.Repositories;

namespace MinimalApplication.Infrastructure.IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
