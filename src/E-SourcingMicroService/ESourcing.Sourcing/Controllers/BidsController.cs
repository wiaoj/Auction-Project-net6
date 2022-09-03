using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Abstracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ESourcing.Sourcing.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BidsController : ControllerBase {
    private readonly IBidRepository _bidRepository;

    public BidsController(IBidRepository bidRepository) {
        this._bidRepository = bidRepository;
    }

    [HttpPost]
    [ProducesResponseType((Int32)HttpStatusCode.OK)]
    public async Task<ActionResult> SendBid([FromBody] Bid bid) {
        await _bidRepository.SendBid(bid);
        return Ok();
    }

    [HttpGet("GetBidByAuctionId")]
    [ProducesResponseType(typeof(IEnumerable<Bid>), (Int32)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Bid>>> GetBidByAuctionId(String auctionId) {
        return Ok(await _bidRepository.GetBidsByAuctionId(auctionId));
    }

    [HttpGet("[action]")] //metodun adını almasını sağlıyor
    [ProducesResponseType(typeof(Bid), (Int32)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Bid>>> GetWinnerBid(String id) {
        return Ok(await _bidRepository.GetWinnerBid(id));
    }
}