using System;

namespace SaaSFulfillmentClient.Models
{
    using System.Collections.Generic;

    public class SubscriptionOperation : FulfillmentRequestResult
    {
        public string Action { get; set; }
        public Guid ActivityId { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorStatusCode { get; set; }
        public Guid Id { get; set; }
        public string OfferId { get; set; }
        public string OperationRequestSource { get; set; }
        public string PlanId { get; set; }
        public string PublisherId { get; set; }
        public string Quantity { get; set; }
        public Uri ResourceLocation { get; set; }
        public OperationStatusEnum Status { get; set; }
        public Guid SubscriptionId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }

    public class SubscriptionOperationResult : FulfillmentRequestResult
    {
        public string ContinuationToken { get; set; }
        public IEnumerable<SubscriptionOperation> Operations { get; set; }
    }
}
