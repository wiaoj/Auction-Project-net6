using ESourcing.Sourcing.Entities;

namespace ESourcing.Sourcing.Repositories.Abstracts;

public interface IBidRepository {
    Task SendBid(Bid bid);
    Task<List<Bid>> GetBidsByAuctionId(String auctionId);
    Task<Bid> GetWinnerBid(String id);
}