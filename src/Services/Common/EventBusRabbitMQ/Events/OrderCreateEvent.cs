using EventBusRabbitMQ.Events.Abstracts;

namespace EventBusRabbitMQ.Events;
public class OrderCreateEvent : IEvent {
    public String Id { get; set; }
    public String AuctionId { get; set; }
    public String ProductId { get; set; }
    public String SellerUserName { get; set; }
    public Decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public Int32 Quantity { get; set; }
}