using ESourcing.Products.Entities;
using MongoDB.Driver;

namespace ESourcing.Products.Data.Abstracts;

public interface IProductContext {
	IMongoCollection<Product> Products { get; }
}