using AutoMapper;
using Orders.Application.Commands.OrderCreate;
using Orders.Application.Responses;
using Orders.Domain.Entities;

namespace Orders.Application.Mappers;
public class OrderMappingProfile : Profile {
    public OrderMappingProfile() {
        CreateMap<Order, OrderCreateCommand>().ReverseMap();
        CreateMap<Order, OrderResponse>().ReverseMap();
    }
}