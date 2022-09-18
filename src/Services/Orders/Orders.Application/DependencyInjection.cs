using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Mappers;
using Orders.Application.PipelineBehaviors;
using System.Reflection;

namespace Orders.Application;
public static class DependencyInjection {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        var config = new MapperConfiguration(cfg => {
            cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
            cfg.AddProfile<OrderMappingProfile>();
        });
        var mapper = config.CreateMapper();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviours<,>));



        return services;
    }
}