using System;
using Newtonsoft.Json;

namespace SaaSFulfillmentClient.Models
{
    public class ResolvedSubscription : FulfillmentRequestResult
    {
        public string OfferId { get; set; }

        public Guid OperationId { get; set; }

        public string PlanId { get; set; }

        public int Quantity { get; set; }

        [JsonProperty("id")] public Guid SubscriptionId { get; set; }

        public string SubscriptionName { get; set; }
    }
}
