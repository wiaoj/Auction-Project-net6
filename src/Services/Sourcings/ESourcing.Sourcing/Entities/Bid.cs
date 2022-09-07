using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ESourcing.Sourcing.Entities;

public class Bid {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public String? Id { get; set; }
	public String AuctionId { get; set; }
	public String ProductId { get; set; }
	public String SellerUserName { get; set; }
	public Decimal Price { get; set; }
	public DateTime CreatedAt { get; set; }
}