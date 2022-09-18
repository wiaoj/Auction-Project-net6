using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Domain.Repositories;
using Orders.Domain.Repositories.Base;
using Orders.Infrastructure.Data;
using Orders.Infrastructure.Repositories;
using Orders.Infrastructure.Repositories.Base;

namespace Orders.Infrastructure;
public static class DependencyInjection {
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) {

        services.AddDbContext<OrderContext>(option => option.UseInMemoryDatabase(databaseName: "InMemoryDb"), ServiceLifetime.Singleton, ServiceLifetime.Singleton);

        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        services.AddTransient<IOrderRepository, OrderRepository>();
        return services;
    }
}