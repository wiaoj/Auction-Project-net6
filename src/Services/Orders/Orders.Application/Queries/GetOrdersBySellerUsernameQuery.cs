using MediatR;
using Orders.Application.Responses;

namespace Orders.Application.Queries;
public class GetOrdersBySellerUsernameQuery : IRequest<IEnumerable<OrderResponse>> {
    public String UserName { get; set; }

    public GetOrdersBySellerUsernameQuery(String userName) {
        UserName = userName;
    }
}