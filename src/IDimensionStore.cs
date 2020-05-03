namespace SaaSFulfillmentClient
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using SaaSFulfillmentClient.AzureTable;
    using SaaSFulfillmentClient.Models;

    public interface IDimensionStore
    {
        Task<IEnumerable<DimensionUsageRecord>> GetAllDimensionRecordsAsync(Guid subscriptionId, CancellationToken cancellationToken = default);

        Task RecordAsync(Guid subscriptionId,
                         CustomMeteringRequestResult result,
                         CancellationToken cancellationToken = default);
    }
}
