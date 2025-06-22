using Microsoft.Extensions.Logging;
using SupplyChain.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SupplyChain.Infrastructure.Services
{
    public class FakeStoreApiClient : IFakeStoreApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FakeStoreApiClient> _logger;

        public FakeStoreApiClient(HttpClient httpClient, ILogger<FakeStoreApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            // API adresi Program.cs'de yapılandırılacak.
        }

        public async Task<IEnumerable<FakeStoreProductResponse>> GetProductsAsync()
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<IEnumerable<FakeStoreProductResponse>>("products");
                return products ?? Enumerable.Empty<FakeStoreProductResponse>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Fake Store API'ye erişirken bir hata oluştu.");
                return Enumerable.Empty<FakeStoreProductResponse>();
            }
        }
    }
}
