using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace EventBusRabbitMQ;

public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection {
    private readonly IConnectionFactory _ConnectionFactory;
    private IConnection _connection;
    private readonly Int32 _retryCount;
    private Boolean _disposed;
    private readonly ILogger<DefaultRabbitMQPersistentConnection> _logger;

    public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory, Int32 retryCount, Boolean disposed, ILogger<DefaultRabbitMQPersistentConnection> logger) {
        this._ConnectionFactory = connectionFactory;
        this._retryCount = retryCount;
        this._disposed = disposed;
        this._logger = logger;
    }

    public Boolean IsConnected {
        get {
            return _connection is not null && _connection.IsOpen && _disposed is false;
        }
    }
    public Boolean TryConnect() {
        //_logger.LogInformation("RabbitMQ Client is trying to connect");

        var policy = RetryPolicy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, time) => {
                _logger.LogWarning(exception, $"RabbitMQ Client cold not connect after {time.TotalSeconds:n1}s {exception.Message}");
            });

        policy.Execute(() => {
            _connection = _ConnectionFactory.CreateConnection();
        });

        if(IsConnected) {
            _connection.ConnectionShutdown += OnConnectionShutdown;
            _connection.CallbackException += OnCallbackException;
            _connection.ConnectionBlocked += OnConnectionBlocked;

            _logger.LogInformation($"RabbitMQ Client acquired a persistent connection to '{_connection.KnownHosts}' and is subscribed to failure events");

            return true;
        }else {
            _logger.LogCritical("FATAL ERROR: RABBİTMQ connections could not be created and opened");
            return false;
        }
    }

    private void OnConnectionBlocked(Object sender, ConnectionBlockedEventArgs eventArgs) {
        if(_disposed)
            return;

        _logger.LogCritical("A RabbitMQ connection is shutdown. Trying to re-connect...");

        TryConnect();
    }

    private void OnCallbackException(Object sender, CallbackExceptionEventArgs e) {
        if(_disposed)
            return;

        _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

        TryConnect();
    }

    private void OnConnectionShutdown(Object sender, ShutdownEventArgs e) {
        if(_disposed)
            return;

        _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

        TryConnect();
    }

    public IModel CreateModel() {
        if(IsConnected is false) {
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
        }

        return _connection.CreateModel();
    }
    
    public void Dispose() {
        if(_disposed)
            return;

        _disposed = true;

        try {
            _connection.Dispose();
        } catch(IOException exception) {

            _logger.LogCritical($"{exception}");
        }
    }
}