using ESourcing.Sourcing.Data.Abstracts;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Settings;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Data;

public class SourcingContext : ISourcingContext {

    public SourcingContext(ISourcingDatabaseSettings settings) {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        Auctions = database.GetCollection<Auction>(nameof(Auction));
        Bids = database.GetCollection<Bid>(nameof(Bid));

        SourcingContextSeed.SeedData(Auctions);
    }

    public IMongoCollection<Auction> Auctions { get; }
    public IMongoCollection<Bid> Bids { get; }
}

public class SourcingContextSeed {
    public static void SeedData(IMongoCollection<Auction> auctionCollection) {
        Boolean exist = auctionCollection.Find(x => true).Any();

        if(exist is false) {
            auctionCollection.InsertManyAsync(GetConfigureAuctions());
        }
    }

    private static IEnumerable<Auction> GetConfigureAuctions() {
        return new List<Auction>() {
            new() {
                Name= "Auction 1",
                Description = "Auction Desc 1",
                CreatedAt = DateTime.Now,
                StartedAt = DateTime.Now,
                FinishedAt = DateTime.Now,
                ProductId = "",
                IncludedSellers = new List<String>() {
                    "bertan@gmail.com",
                    "bertan@bertan.com"
                },
                Quantity = 5,
                Status = Convert.ToInt32(Status.Active)

            },
            new() {
                Name= "Auction 2",
                Description = "Auction Desc 2",
                CreatedAt = DateTime.Now,
                StartedAt = DateTime.Now,
                FinishedAt = DateTime.Now,
                ProductId = "",
                IncludedSellers = new List<String>() {
                    "bertan@gmail.com",
                    "bertan@bertan.com"
                },
                Quantity = 5,
                Status = Convert.ToInt32(Status.Active)

            },
            new() {
                Name= "Auction 3",
                Description = "Auction Desc 3",
                CreatedAt = DateTime.Now,
                StartedAt = DateTime.Now,
                FinishedAt = DateTime.Now,
                ProductId = "",
                IncludedSellers = new List<String>() {
                    "bertan@gmail.com",
                    "bertan@bertan.com"
                },
                Quantity = 5,
                Status = Convert.ToInt32(Status.Active)

            }
        };
    }
}