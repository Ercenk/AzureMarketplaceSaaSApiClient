using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SaaSFulfillmentClient.Models;

namespace SaaSFulfillmentClient
{
    public interface IFulfillmentClient
    {
        Task<FulfillmentRequestResult> ActivateSubscriptionAsync(Guid subscriptionId,
            ActivatedSubscription subscriptionDetails, Guid requestId, Guid correlationId,
            CancellationToken cancellationToken);

        Task<UpdateOrDeleteSubscriptionRequestResult> DeleteSubscriptionAsync(Guid subscriptionId, Guid requestId,
            Guid correlationId, CancellationToken cancellationToken);

        Task<IEnumerable<SubscriptionOperation>> GetOperationsAsync(Guid requestId, Guid correlationId,
            CancellationToken cancellationToken);

        Task<Subscription> GetSubscriptionAsync(Guid subscriptionId, Guid requestId, Guid correlationId,
            CancellationToken cancellationToken);

        Task<SubscriptionOperation> GetSubscriptionOperationAsync(Guid subscriptionId, Guid operationId, Guid requestId,
            Guid correlationId, CancellationToken cancellationToken);

        Task<IEnumerable<SubscriptionOperation>> GetSubscriptionOperationsAsync(Guid subscriptionId, Guid requestId,
            Guid correlationId, CancellationToken cancellationToken);

        Task<SubscriptionPlans> GetSubscriptionPlansAsync(Guid subscriptionId, Guid requestId, Guid correlationId,
            CancellationToken cancellationToken);

        Task<IEnumerable<Subscription>> GetSubscriptionsAsync(Guid requestId, Guid correlationId,
            CancellationToken cancellationToken);

        Task<ResolvedSubscription> ResolveSubscriptionAsync(string marketplaceToken, Guid requestId, Guid correlationId,
            CancellationToken cancellationToken);

        Task<UpdateOrDeleteSubscriptionRequestResult> UpdateSubscriptionAsync(Guid subscriptionId,
            ActivatedSubscription update, Guid requestId, Guid correlationId, CancellationToken cancellationToken);
    }
}
