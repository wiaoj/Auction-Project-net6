using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using Orders.Domain.Repositories;
using Orders.Infrastructure.Data;
using Orders.Infrastructure.Repositories.Base;

namespace Orders.Infrastructure.Repositories;
public class OrderRepository : Repository<Order>, IOrderRepository {
    public OrderRepository(OrderContext context) : base(context) { }

    public async Task<IEnumerable<Order>> GetOrdersBySellerUserName(String userName)
        => await _context.Orders.Where(o => o.SellerUserName.Equals(userName))
        .ToListAsync();
}