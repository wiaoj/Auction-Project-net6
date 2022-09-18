using Orders.Domain.Entities;

namespace Orders.Infrastructure.Data;

public class OrderContextSeed {
    public static async Task SeedAsync(OrderContext orderContext) {
        if(orderContext.Orders.Any() is false) {
            orderContext.Orders.AddRange(GetPreconfiguredOrders());
            await orderContext.SaveChangesAsync();
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders() {
        return new List<Order>() {
            new() {
                AuctionId = $"{Guid.NewGuid()}",
                ProductId = $"{Guid.NewGuid()}",
                SellerUserName = "test@test.com",
                UnitPrice = 10,
                TotalPrice = 1_000,
                CreateAt = DateTime.UtcNow
            },
            new() {
                AuctionId = $"{Guid.NewGuid()}",
                ProductId = $"{Guid.NewGuid()}",
                SellerUserName = "test@test.com",
                UnitPrice = 10,
                TotalPrice = 120_000,
                CreateAt = DateTime.UtcNow
            },
            new() {
                AuctionId = $"{Guid.NewGuid()}",
                ProductId = $"{Guid.NewGuid()}",
                SellerUserName = "test@test.com",
                UnitPrice = 10,
                TotalPrice = 9_999_999,
                CreateAt = DateTime.UtcNow
            }
        };
    }
}