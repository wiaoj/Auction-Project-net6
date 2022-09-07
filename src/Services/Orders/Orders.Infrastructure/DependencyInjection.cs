using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Infrastructure.Data;

namespace Orders.Infrastructure;
public static class DependencyInjection {
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) {

        services.AddDbContext<OrderContext>(option => option.UseInMemoryDatabase(databaseName: "InMemoryDb"), ServiceLifetime.Singleton, ServiceLifetime.Singleton);
        return services;
    }
}