﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SaaSFulfillmentClient.Models;

namespace SaaSFulfillmentClient
{
    public interface IFulfillmentClient
    {
        Task<FullfilmentRequestResult> ActivateSubscriptionAsync(Guid subscriptionId, ActivatedSubscription subscriptionDetails, Guid requestId, Guid correlationId, string bearerToken, CancellationToken cancellationToken);

        Task<UpdateOrDeleteSubscriptionRequestResult> DeleteSubscriptionAsync(Guid subscriptionId, Guid requestId, Guid correlationId, string bearerToken, CancellationToken cancellationToken);

        Task<IEnumerable<SubscriptionOperation>> GetOperationsAsync(Guid requestId, Guid correlationId, string bearerToken, CancellationToken cancellationToken);

        Task<Subscription> GetSubscriptionAsync(Guid subscriptionId, Guid requestId, Guid correlationId, string bearerToken, CancellationToken cancellationToken);

        Task<SubscriptionOperation> GetSubscriptionOperationAsync(Guid subscriptionId, string operationId, Guid requestId, Guid correlationId, string bearerToken, CancellationToken cancellationToken);

        Task<IEnumerable<SubscriptionOperation>> GetSubscriptionOperationsAsync(Guid subscriptionId, Guid requestId, Guid correlationId, string bearerToken, CancellationToken cancellationToken);

        Task<SubscriptionPlans> GetSubscriptionPlansAsync(Guid subscriptionId, Guid requestId, Guid correlationId, string bearerToken, CancellationToken cancellationToken);

        Task<IEnumerable<Subscription>> GetSubscriptionsAsync(Guid requestId, Guid correlationId, string bearerToken, CancellationToken cancellationToken);

        Task<ResolvedSubscription> ResolveSubscriptionAsync(string marketplaceToken, Guid requestId, Guid correlationId, string bearerToken, CancellationToken cancellationToken);

        Task<UpdateOrDeleteSubscriptionRequestResult> UpdateSubscriptionAsync(Guid subscriptionId, ActivatedSubscription update, Guid requestId, Guid correlationId, string bearerToken, CancellationToken cancellationToken);
    }
}
