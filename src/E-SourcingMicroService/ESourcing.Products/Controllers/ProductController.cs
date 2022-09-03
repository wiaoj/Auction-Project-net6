using ESourcing.Products.Entities;
using ESourcing.Products.Respositories.Abstracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.CompilerServices;

namespace ESourcing.Products.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductsController : ControllerBase {
	#region Variables
	private readonly IProductRepository _productRepository;
	//private readonly ILogger _logger;
	#endregion

	#region Constructor
	public ProductsController(IProductRepository productRepository/*, ILogger logger*/) {
		this._productRepository = productRepository;
		//this._logger = logger;
	}
	#endregion

	#region Crud_Actions
	[HttpGet]
	[ProducesResponseType(typeof(Product), (Int32)HttpStatusCode.OK)]
	public async Task<ActionResult<IEnumerable<Product>>> GetProducts() {
		var products = await _productRepository.GetProducts();
		return Ok(products);
	}

	[HttpGet("{id:length(24)}", Name = "GetProduct")]
	[ProducesResponseType(typeof(Product), (Int32)HttpStatusCode.OK)]
	[ProducesResponseType( (Int32)HttpStatusCode.NotFound)]
	public async Task<ActionResult<Product>> GetProductById(String id) {
		var product = await _productRepository.GetProduct(id);
		return product is null ? NotFound() : Ok(product);
	}

	[HttpPost]
	[ProducesResponseType(typeof(Product), (Int32)HttpStatusCode.Created)]
	public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product) {
		await _productRepository.Create(product);
		return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
	}

	[HttpPut]
	[ProducesResponseType(typeof(Product), (Int32)HttpStatusCode.OK)]
	public async Task<IActionResult> UpdateProduct([FromBody] Product product) {
		return Ok(await _productRepository.Update(product));
	}

	[HttpDelete("{id:length(24)}")]
	[ProducesResponseType(typeof(Product), (Int32)HttpStatusCode.OK)]
	public async Task<IActionResult> DeleteProductById(String id) {
		return Ok(await _productRepository.Delete(id));
	}
	#endregion
}
