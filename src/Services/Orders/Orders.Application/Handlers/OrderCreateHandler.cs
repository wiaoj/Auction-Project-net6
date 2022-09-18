using AutoMapper;
using MediatR;
using Orders.Application.Commands.OrderCreate;
using Orders.Application.Responses;
using Orders.Domain.Entities;
using Orders.Domain.Repositories;

namespace Orders.Application.Handlers;
internal class OrderCreateHandler : IRequestHandler<OrderCreateCommand, OrderResponse> {
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderCreateHandler(IOrderRepository orderRepository, IMapper mapper) {
        this._orderRepository = orderRepository;
        this._mapper = mapper;
    }

    public async Task<OrderResponse> Handle(OrderCreateCommand request, CancellationToken cancellationToken) {
        Order orderEntity = _mapper.Map<Order>(request);
        if(orderEntity is null)
            throw new ApplicationException("Entity could not be mapped");

        Order createdOrder = await _orderRepository.AddAsync(orderEntity);

        OrderResponse response = _mapper.Map<OrderResponse>(createdOrder);
        return response;
    }
}