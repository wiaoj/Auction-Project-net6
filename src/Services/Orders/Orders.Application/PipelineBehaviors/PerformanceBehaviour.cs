﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Orders.Application.PipelineBehaviors;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> {
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;

    public PerformanceBehaviour(ILogger<TRequest> logger) {
        _timer = new Stopwatch();
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if(elapsedMilliseconds > 500)
            _logger.LogWarning(message: $"Long Runnig Request: {typeof(TRequest).Name} ({elapsedMilliseconds} milliseconds) {request}");


        return response;
    }
}