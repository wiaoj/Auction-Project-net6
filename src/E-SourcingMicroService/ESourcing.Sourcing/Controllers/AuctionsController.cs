using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Abstracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ESourcing.Sourcing.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class AuctionsController : ControllerBase {
    private readonly IAuctionRepository _auctionRepository;

    public AuctionsController(IAuctionRepository auctionRepository) {
        this._auctionRepository = auctionRepository;
    }

    // GET: api/<ValuesController>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Auction>), (Int32)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Auction>>> GetAuction() {
        return Ok(await _auctionRepository.GetAuctions());
    }

    // GET api/<ValuesController>/5
    [HttpGet("{id:length(24)}", Name = "GetAuction")]
    [ProducesResponseType(typeof(Auction), (Int32)HttpStatusCode.OK)]
    [ProducesResponseType((Int32)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Auction>> Get(String id) {
        var auction = await _auctionRepository.GetAuction(id);
        return auction is null ? NotFound() : Ok(auction);
    }

    // POST api/<ValuesController>
    [HttpPost]
    [ProducesResponseType(typeof(Auction), (Int32)HttpStatusCode.Created)]
    public async Task<ActionResult<Auction>> CreateAuction([FromBody] Auction auction) {
        await _auctionRepository.Create(auction);
        return CreatedAtAction(nameof(GetAuction), new {
            id = auction.Id
        }, auction);
    }

    // PUT api/<ValuesController>/5
    [HttpPut]
    [ProducesResponseType(typeof(Auction), (Int32)HttpStatusCode.OK)]
    public async Task<ActionResult<Auction>> UpdateAuction([FromBody] Auction auction) {
        return Ok(await _auctionRepository.Update(auction));
    }

    // DELETE api/<ValuesController>/5
    [HttpDelete("{id:length(24)}")]
    [ProducesResponseType(typeof(Auction), (Int32)HttpStatusCode.OK)]
    public async Task<ActionResult<Auction>> Delete(String id) {
        return Ok(await _auctionRepository.Delete(id));
    }
}
