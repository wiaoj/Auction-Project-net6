using MediatR;
using Microsoft.Extensions.Logging;

namespace Orders.Application.PipelineBehaviors;

public class UnhandledExceptionBehaviours<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> {
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviours(ILogger<TRequest> logger) {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {
        try {
            return await next();
        } catch(Exception exception) {
            _logger.LogError(exception, $"CleanArchitecture Request: Unhandled Exception for Request {typeof(TRequest).Name} {request}");
            throw;
        }
    }
}