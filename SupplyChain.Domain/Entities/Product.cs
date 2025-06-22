using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyChain.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }

        // Fake Store API'deki ürünlerle eşleştirmek için kullanılacak benzersiz kod.
        // Bu, işe alım görevinde belirtilen `productCode` alanı.
        public string ProductCode { get; private set; }

        public string Name { get; private set; }

        public int StockQuantity { get; private set; }

        public int StockThreshold { get; private set; }

        // Private constructor, Entity Framework Core gibi ORM'ler için gereklidir.
        private Product() { }

        // Yeni bir ürün oluşturmak için factory metodu.
        public static Product Create(int id, string name, int stockQuantity, int stockThreshold)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Ürün adı boş olamaz.", nameof(name));
            if (stockQuantity < 0)
                throw new ArgumentException("Başlangıç stoğu negatif olamaz.", nameof(stockQuantity));
            if (stockThreshold < 0)
                throw new ArgumentException("Stok eşik değeri negatif olamaz.", nameof(stockThreshold));

            // ProductCode'u basitçe Id'den veya başka bir kuraldan türetebiliriz.
            // Fake Store API'deki ürün ID'leri ile eşleştirmek için kullanacağız.
            return new Product
            {
                Id = id,
                ProductCode = $"FS-{id}", // Fake Store API'deki ID ile eşleşecek format.
                Name = name,
                StockQuantity = stockQuantity,
                StockThreshold = stockThreshold
            };
        }

        /// <summary>
        /// Ürünün stok miktarını günceller.
        /// </summary>
        /// <param name="newQuantity">Yeni stok miktarı.</param>
        public void UpdateStock(int newQuantity)
        {
            if (newQuantity < 0)
                throw new InvalidOperationException("Stok miktarı negatif olamaz.");

            StockQuantity = newQuantity;
        }

        /// <summary>
        /// Ürünün kritik stok seviyesinde olup olmadığını kontrol eder.
        /// </summary>
        public bool IsInLowStock() => StockQuantity < StockThreshold;
    }
}
