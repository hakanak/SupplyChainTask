using SupplyChain.Contracts.Services;
using SupplyChain.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyChain.Infrastructure.Persistence
{
    public class InMemoryProductRepository : IProductRepository
    {
        // Thread-safe bir koleksiyon kullanalım ki eş zamanlı isteklerde sorun çıkmasın.
        private static readonly ConcurrentDictionary<int, Product> _products = new();
        private static int _nextId = 1;

        public Task AddAsync(Product product)
        {
            // Gerçek bir senaryoda bu ID veritabanından gelirdi.
            // Burada basitçe biz atıyoruz.
            var newId = Interlocked.Increment(ref _nextId);
            var productToAdd = Product.Create(newId, product.Name, product.StockQuantity, product.StockThreshold);

            _products[newId] = productToAdd;
            return Task.CompletedTask;
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            _products.TryGetValue(id, out var product);
            return Task.FromResult(product);
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Product>>(_products.Values.ToList());
        }

        public Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            var lowStockProducts = _products.Values.Where(p => p.IsInLowStock()).ToList();
            return Task.FromResult<IEnumerable<Product>>(lowStockProducts);
        }
    }
}
