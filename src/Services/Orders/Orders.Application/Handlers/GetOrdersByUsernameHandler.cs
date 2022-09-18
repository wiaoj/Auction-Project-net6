using AutoMapper;
using MediatR;
using Orders.Application.Queries;
using Orders.Application.Responses;
using Orders.Domain.Entities;
using Orders.Domain.Repositories;

namespace Orders.Application.Handlers;
internal class GetOrdersByUsernameHandler : IRequestHandler<GetOrdersBySellerUsernameQuery, IEnumerable<OrderResponse>> {
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersByUsernameHandler(IOrderRepository orderRepository, IMapper mapper) {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersBySellerUsernameQuery request, CancellationToken cancellationToken) {
        IEnumerable<Order> orderList = await _orderRepository.GetOrdersBySellerUserName(request.UserName);

        IEnumerable<OrderResponse> orderResponse = _mapper.Map<IEnumerable<OrderResponse>>(orderList);

        return orderResponse;
    }
}