using Microsoft.EntityFrameworkCore;
using Orders.Infrastructure.Data;

namespace ESourcing.Order.Extensions;

public static class MigrationManager {
    public static IHost MigrateDatabase(this IHost host) {
        using var scope = host.Services.CreateScope();

        try {
            var orderContext = scope.ServiceProvider.GetRequiredService<OrderContext>();

            if(orderContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory") {
                orderContext.Database.Migrate();
            }
            OrderContextSeed.SeedAsync(orderContext).Wait();
        } catch(Exception exception) {

            throw;
        }

        return host;
    }
}