using Microsoft.AspNetCore.Mvc;
using SupplyChain.Application.Services;

namespace SupplyChain.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Kritik seviyedeki ürünleri kontrol eder ve en uygun tedarikçiden sipariş oluşturur
        /// </summary>
        /// <returns>Oluşturulan siparişlerin bir özetini döncek</returns>
        [HttpPost("check-and-place")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckAndPlaceOrders()
        {
            var result = await _orderService.CheckAndPlaceOrdersForLowStockItemsAsync();
            return Ok(result);
        }
    }
}
