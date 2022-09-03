using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ESourcing.Sourcing.Entities;

public class Auction {
    public Auction() {
        IncludedSellers = new List<String>();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public String? Id { get; set; }
    public String Name { get; set; }
    public String Description { get; set; }
    public String ProductId { get; set; }
    public Int32 Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime FinishedAt { get; set; }
    public Int32 Status { get; set; }
    public List<String> IncludedSellers { get; set; }
}
public enum Status {
    Active = 0,
    Closed = 1,
    Passive = 2
}