using Orders.Domain.Entities;
using Orders.Domain.Repositories.Base;

namespace Orders.Domain.Repositories;
public interface IOrderRepository : IRepository<Order> {
    Task<IEnumerable<Order>> GetOrdersBySellerUserName(String userName);
}