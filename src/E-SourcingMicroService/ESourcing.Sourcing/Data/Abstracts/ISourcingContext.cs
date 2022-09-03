using ESourcing.Sourcing.Entities;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Data.Abstracts;

public interface ISourcingContext {
    IMongoCollection<Auction> Auctions { get; }
    IMongoCollection<Bid> Bids { get; }
}