using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ESourcing.Products.Entities;

public class Product {
	//mongoDb id değeri string olmalıdır
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)] //id'nin otomatik artmasını sağlayan özellik
	public String? Id { get; set; }

	[BsonElement("Name")]
	public String Name { get; set; }
	public String Category { get; set; }
	public String Summary { get; set; }
	public String Description { get; set; }
	public String ImageFile { get; set; }
	public Decimal Price { get; set; }
}