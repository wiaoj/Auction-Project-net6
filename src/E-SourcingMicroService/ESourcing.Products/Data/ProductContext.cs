using ESourcing.Products.Data.Abstracts;
using ESourcing.Products.Entities;
using ESourcing.Products.Settings;
using MongoDB.Driver;

namespace ESourcing.Products.Data;

public class ProductContext : IProductContext {
    public ProductContext(IProductDatabaseSettings settings) {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        Products = database.GetCollection<Product>(settings.CollectionName);
        ProductContextSeed.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; }
}

public class ProductContextSeed {
    public static void SeedData(IMongoCollection<Product> productCollection) {
        Boolean existProduct = productCollection.Find(x => true).Any();

        if(existProduct is false) {
            productCollection.InsertManyAsync(GetConfigureProducts());
        }
    }

    private static IEnumerable<Product> GetConfigureProducts() {
        return new List<Product>() {
            new() {
                Name = "Samsung S20",
                Summary = "This phone is the company's biggest change to ....",
                Description = "Lorem ipsum dolor sit amet...",
                ImageFile= "product-2.png",
                Price = 840.00M,
                Category = "Smart Phone"
            },
            new() {
                Name = "Samsung S20+",
                Summary = "This phone is the company's biggest change to ....",
                Description = "Lorem ipsum dolor sit amet...",
                ImageFile= "product-2.png",
                Price = 640.00M,
                Category = "White Appliances Phone"
            }
        };
    }
}