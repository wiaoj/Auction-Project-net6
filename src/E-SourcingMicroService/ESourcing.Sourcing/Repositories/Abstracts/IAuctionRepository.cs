using ESourcing.Sourcing.Entities;

namespace ESourcing.Sourcing.Repositories.Abstracts;

public interface IAuctionRepository {
    Task<IEnumerable<Auction>> GetAuctions();
    Task<Auction> GetAuction(String id);
    Task<Auction> GetAuctionByName(String name);
    Task Create(Auction auction);
    Task<Boolean> Update(Auction auction);
    Task<Boolean> Delete(String id);
}