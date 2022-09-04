namespace EventBusRabbitMQ.Events.Abstracts;
public abstract class IEvent {
    public Guid RequestId { get; set; }
    public DateTime CreationDate { get; set; }

    public IEvent() {
        RequestId = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }
}