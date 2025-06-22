using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyChain.Contracts.Services
{
    public record FakeStoreProductResponse(
        int Id,
        string Title,
        decimal Price,
        string Description,
        string Category,
        string Image
    );

    public interface IFakeStoreApiClient
    {
        Task<IEnumerable<FakeStoreProductResponse>> GetProductsAsync();
    }
}
