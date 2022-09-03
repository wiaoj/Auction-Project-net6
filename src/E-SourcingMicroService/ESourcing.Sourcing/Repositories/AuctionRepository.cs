using ESourcing.Sourcing.Data.Abstracts;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Abstracts;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Repositories;

public class AuctionRepository : IAuctionRepository {
    private readonly ISourcingContext _context;

    public AuctionRepository(ISourcingContext context) {
        this._context = context;
    }

    public async Task Create(Auction auction) {
        await _context.Auctions.InsertOneAsync(auction);
    }

    public async Task<Boolean> Delete(String id) {
        FilterDefinition<Auction> filter = Builders<Auction>.Filter.Eq(x => x.Id, id);
        DeleteResult deleteResult = await _context.Auctions.DeleteOneAsync(filter);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }

    public async Task<Auction> GetAuction(String id) {
        return await _context.Auctions.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public async Task<Auction> GetAuctionByName(String name) {
        FilterDefinition<Auction> filter = Builders<Auction>.Filter.Eq(x => x.Name, name);
        return await _context.Auctions.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Auction>> GetAuctions() {
        return await _context.Auctions.Find(x => true).ToListAsync();
    }

    public async Task<Boolean> Update(Auction auction) {
        var updatedResult = await _context.Auctions.ReplaceOneAsync(x => x.Id.Equals(auction.Id), auction);
        return updatedResult.IsAcknowledged && updatedResult.ModifiedCount > 0;
    }
}