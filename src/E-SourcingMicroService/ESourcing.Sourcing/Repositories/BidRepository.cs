using ESourcing.Sourcing.Data.Abstracts;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Abstracts;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Repositories;

public class BidRepository : IBidRepository {
    private readonly ISourcingContext _context;

    public BidRepository(ISourcingContext context) {
        this._context = context;
    }

    public async Task<List<Bid>> GetBidsByAuctionId(String auctionId) {
        FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(x => x.AuctionId, auctionId);
        List<Bid> bids = await _context.Bids.Find(filter).ToListAsync();

        bids = bids.OrderByDescending(x => x.CreatedAt)
            .GroupBy(seller => seller.SellerUserName)
            .Select(x => new Bid {
                AuctionId = x.FirstOrDefault().AuctionId,
                Price = x.FirstOrDefault().Price,
                CreatedAt = x.FirstOrDefault().CreatedAt,
                SellerUserName = x.FirstOrDefault().SellerUserName,
                ProductId = x.FirstOrDefault().ProductId,
                Id = x.FirstOrDefault().Id
            }).ToList();
        return bids;
    }

    public async Task<Bid> GetWinnerBid(String id) {
        List<Bid> bids = await GetBidsByAuctionId(id);

        return bids.OrderByDescending(x => x.Price).FirstOrDefault();
    }

    public async Task SendBid(Bid bid) {
        await _context.Bids.InsertOneAsync(bid);
    }
}