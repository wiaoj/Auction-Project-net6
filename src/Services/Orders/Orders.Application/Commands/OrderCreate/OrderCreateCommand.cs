using MediatR;
using Orders.Application.Responses;

namespace Orders.Application.Commands.OrderCreate;
public class OrderCreateCommand : IRequest<OrderResponse> {
    public String AuctionId { get; set; }
    public String SellerUserName { get; set; }
    public String ProductId { get; set; }
    public Decimal UnitPrice { get; set; }
    public Decimal TotalPrice { get; set; }
    public DateTime CreateAt { get; set; }
}