using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyChain.Contracts.Responses
{
    public record ProductResponse(
       int Id,
       string ProductCode,
       string Name,
       int StockQuantity,
       int StockThreshold
   );
}
