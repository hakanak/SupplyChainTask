using SupplyChain.Contracts.Requests;
using SupplyChain.Contracts.Responses;
using SupplyChain.Contracts.Services;
using SupplyChain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyChain.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request)
        {
           
            // ID'yi repository katmanı belirleyeceği için burda 0 gönderiyoruz.
            var product = Product.Create(0, request.Name, request.InitialStock, request.StockThreshold);

            await _productRepository.AddAsync(product);

         
            var allProducts = await _productRepository.GetAllAsync();
            var createdProduct = allProducts.Last(); 

            return new ProductResponse(
                createdProduct.Id,
                createdProduct.ProductCode,
                createdProduct.Name,
                createdProduct.StockQuantity,
                createdProduct.StockThreshold
            );
        }

        public async Task<IEnumerable<ProductResponse>> GetLowStockProductsAsync()
        {
            var lowStockProducts = await _productRepository.GetLowStockProductsAsync();

            return lowStockProducts.Select(p => new ProductResponse(
                p.Id,
                p.ProductCode,
                p.Name,
                p.StockQuantity,
                p.StockThreshold
            ));
        }
    }
}
