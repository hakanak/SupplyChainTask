using Microsoft.AspNetCore.Mvc;
using SupplyChain.Application.Services;
using SupplyChain.Contracts.Requests;

namespace SupplyChain.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Sisteme yeni bir ürün ekler.
        /// </summary>
        /// <param name="request">Ürün adı, başlangıç stoğu ve stok eşik değeri.</param>
        /// <returns>Oluşturulan ürünün bilgileri.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            var productResponse = await _productService.CreateProductAsync(request);
            return CreatedAtAction(nameof(GetProductById), new { id = productResponse.Id }, productResponse);
        }

        /// <summary>
        /// Kritik stok seviyesinin altındaki ürünleri listeler.
        /// </summary>
        [HttpGet("low-stock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var products = await _productService.GetLowStockProductsAsync();
            return Ok(products);
        }

        // Bu endpoint sadece CreatedAtAction'ın çalışması için var
        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)] // Swagger'da dan gizliyelim
        public IActionResult GetProductById(int id)
        {
            return Ok($" id : {id}   ürün burada ");
        }
    }
}
