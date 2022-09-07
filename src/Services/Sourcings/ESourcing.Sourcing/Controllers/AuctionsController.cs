using AutoMapper;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories;
using ESourcing.Sourcing.Repositories.Abstracts;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ESourcing.Sourcing.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class AuctionsController : ControllerBase {
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;
    private readonly IMapper _mapper;
    private readonly EventBusRabbitMQProducer _eventBus;
    private readonly ILogger<AuctionRepository> _logger;

    public AuctionsController(IAuctionRepository auctionRepository, IBidRepository bidRepository, IMapper mapper, ILogger<AuctionRepository> logger, EventBusRabbitMQProducer eventBus) {
        this._auctionRepository = auctionRepository;
        this._bidRepository = bidRepository;
        this._mapper = mapper;
        this._logger = logger;
        this._eventBus = eventBus;
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

    [HttpPost("[action]/{id:length(24)}")]
    [ProducesResponseType((Int32)HttpStatusCode.NotFound)]
    [ProducesResponseType((Int32)HttpStatusCode.BadRequest)]
    [ProducesResponseType((Int32)HttpStatusCode.Accepted)]
    public async Task<ActionResult> CompleteAuction(String id) {
        Auction auction = await _auctionRepository.GetAuction(id);
        if(auction is null) {
            return NotFound();
        }

        if(auction.Status is not (Int32)Status.Active) {
            _logger.LogError("Auction can not be completed");
            return BadRequest();
        }

        Bid bid = await _bidRepository.GetWinnerBid(id);
        if(bid is null) {
            return NotFound();
        }

        OrderCreateEvent eventMessage = _mapper.Map<OrderCreateEvent>(bid);
        eventMessage.Quantity = auction.Quantity;

        auction.Status = (Int32)Status.Closed;
        Boolean updateResponse = await _auctionRepository.Update(auction);

        if(updateResponse is false) {
            _logger.LogError("Auction can not updated");
            return BadRequest();
        }

        try {
            _eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
        } catch(Exception exception) {
            _logger.LogError(exception, $"ERROR Publishing integration event: {eventMessage.Id} from Sourcing");
            throw;
        }

        return Accepted();
    }

    [HttpPost("[action]")]
    public ActionResult<OrderCreateEvent> TestEvent() {
        OrderCreateEvent eventMessage = new();

        eventMessage.AuctionId = "dummy1";
        eventMessage.ProductId = "dummy_product_1";
        eventMessage.Price = 10;
        eventMessage.Quantity = 1000;
        eventMessage.SellerUserName = "test@test.com";

        try {
            _eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
        } catch(Exception exception) {
            _logger.LogError(exception, $"ERROR Publishing integration event: {eventMessage.Id} from Sourcing");
            throw;
        }

        return Accepted(eventMessage);
    }
}
