namespace SaaSFulfillmentClient
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using SaaSFulfillmentClient.AzureTable;
    using SaaSFulfillmentClient.Models;

    public interface IOperationsStore
    {
        Task<IEnumerable<OperationRecord>> GetAllSubscriptionRecordsAsync(Guid subscriptionId, CancellationToken cancellationToken = default);

        Task RecordAsync(
                            Guid subscriptionId,
            UpdateOrDeleteSubscriptionRequestResult result,
            CancellationToken cancellationToken = default);
    }
}
