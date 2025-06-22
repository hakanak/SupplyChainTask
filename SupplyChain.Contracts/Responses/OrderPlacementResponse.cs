using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyChain.Contracts.Responses
{
    public record OrderPlacementResponse(
        string Message,
        List<PlacedOrderDetail> PlacedOrders
    );

    public record PlacedOrderDetail(
        string ProductName,
        string ProductCode,
        int QuantityOrdered,
        decimal Price
    );
}
