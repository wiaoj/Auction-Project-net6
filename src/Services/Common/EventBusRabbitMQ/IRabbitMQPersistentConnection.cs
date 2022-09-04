using RabbitMQ.Client;

namespace EventBusRabbitMQ;
public interface IRabbitMQPersistentConnection : IDisposable {
    public Boolean IsConnected { get; }
    public Boolean TryConnect();
    public IModel CreateModel();
}