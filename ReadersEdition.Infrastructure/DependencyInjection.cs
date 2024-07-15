using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, ApplicationDbContext>();
        services.AddScoped<IDictionaryRetriever, WiktionaryClient>();
        return services;
    }
}