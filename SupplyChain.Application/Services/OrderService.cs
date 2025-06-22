using Microsoft.Extensions.Logging;
using SupplyChain.Contracts.Responses;
using SupplyChain.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyChain.Application.Services
{
    public class OrderService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFakeStoreApiClient _fakeStoreApiClient;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IProductRepository productRepository,
            IFakeStoreApiClient fakeStoreApiClient,
            ILogger<OrderService> logger)
        {
            _productRepository = productRepository;
            _fakeStoreApiClient = fakeStoreApiClient;
            _logger = logger;
        }

        public async Task<OrderPlacementResponse> CheckAndPlaceOrdersForLowStockItemsAsync()
        {
            _logger.LogInformation("Sipariş süreci başlatıldı");

            var lowStockProducts = await _productRepository.GetLowStockProductsAsync();
            if (!lowStockProducts.Any())
            {
                return new OrderPlacementResponse("Kritik stok seviyesinde ürün yok", new List<PlacedOrderDetail>());
            }

            var allExternalProducts = await _fakeStoreApiClient.GetProductsAsync();
            if (!allExternalProducts.Any())
            {
                _logger.LogWarning("FakeStoreAPI'den hiçbir ürün alınamadı ve sipariş işlemi yapılamadı");
                return new OrderPlacementResponse("Tedarikçi servisinden ürünler alınamadı", new List<PlacedOrderDetail>());
            }

            var placedOrders = new List<PlacedOrderDetail>();

            foreach (var internalProduct in lowStockProducts)
            {
               
                // Görevde productCode ile eşleştirin denmiş. FS-{id} formatını kullandım
                // FakeStoreAPI'deki id ile bizim productCode umuzu eşleştirdim.
                var matchingExternalProduct = allExternalProducts
                    .Where(extProd => $"FS-{extProd.Id}" == internalProduct.ProductCode)
                    .OrderBy(extProd => extProd.Price) // En ucuz olanı bulmak için sırala
                    .FirstOrDefault();

                if (matchingExternalProduct != null)
                {
                    
                    _logger.LogInformation(
                        "Sipariş veriliyor: Ürün='{ProductName}', Tedarikçi Ürün ID='{ExternalId}', Fiyat='{Price}'",
                        internalProduct.Name, matchingExternalProduct.Id, matchingExternalProduct.Price);

                    
                    placedOrders.Add(new PlacedOrderDetail(
                        internalProduct.Name,
                        internalProduct.ProductCode,
                        internalProduct.StockThreshold - internalProduct.StockQuantity, // Ne kadar sipariş verileceği
                        matchingExternalProduct.Price
                    ));
                }
                else
                {
                    _logger.LogWarning(
                        "Kritik stoktaki '{ProductName}' (Kod: {ProductCode}) için tedarikçide eşleşen ürün yok",
                        internalProduct.Name, internalProduct.ProductCode);
                }
            }

            return new OrderPlacementResponse("Sipariş kontrolü tamamlandı.", placedOrders);
        }
    }
}
