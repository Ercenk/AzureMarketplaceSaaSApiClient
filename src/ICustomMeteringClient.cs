using System;
using System.Collections.Generic;
using System.Text;

namespace SaaSFulfillmentClient
{
    using System.Threading;
    using System.Threading.Tasks;

    using SaaSFulfillmentClient.Models;

    public interface ICustomMeteringClient
    {
        Task<CustomMeteringRequestResult> RecordBatchUsageAsync(
            Guid requestId,
            Guid correlationId,
            IEnumerable<Usage> usage,
            CancellationToken cancellationToken);

        Task<CustomMeteringRequestResult> RecordUsageAsync(
                    Guid requestId,
            Guid correlationId,
            Usage usage,
            CancellationToken cancellationToken);
    }
}
