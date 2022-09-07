using EventBusRabbitMQ.Events.Abstracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;

namespace EventBusRabbitMQ.Producer;
public class EventBusRabbitMQProducer {
    private readonly IRabbitMQPersistentConnection _persistentConnection;
    private readonly ILogger<EventBusRabbitMQProducer> _logger;
    private readonly Int32 _retryCount;

    public EventBusRabbitMQProducer(IRabbitMQPersistentConnection persistentConnection, ILogger<EventBusRabbitMQProducer> logger, Int32 retryCount = 5) {
        _persistentConnection = persistentConnection;
        _logger = logger;
        _retryCount = retryCount;
    }

    public void Publish(String queueName, IEvent @event) {
        if(_persistentConnection.IsConnected is false) {
            _persistentConnection.TryConnect();
        }
        var policy = RetryPolicy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, time) => {
                _logger.LogWarning(exception, $"RabbitMQ Client cold not connect after {time.TotalSeconds:n1}s {exception.Message}");
            });
        using var channel = _persistentConnection.CreateModel();
        channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);

        policy.Execute(() => {
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 2;

            channel.ConfirmSelect();
            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                mandatory: true,
                basicProperties: properties,
                body: body
                );
            channel.WaitForConfirmsOrDie();

            channel.BasicAcks += (sender, eventArgs) => {
                Console.WriteLine("Send RabbitMQ");
            };
        });
    }
}