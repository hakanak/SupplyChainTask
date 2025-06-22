using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyChain.Contracts.Requests
{
    public record CreateProductRequest(
        string Name,
        int InitialStock,
        int StockThreshold
    );
}
