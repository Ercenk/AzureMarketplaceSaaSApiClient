using System.Collections.Generic;

namespace SaaSFulfillmentClient.Models
{
    public class Plan
    {
        public string DisplayName { get; set; }
        public bool IsPrivate { get; set; }
        public string PlanId { get; set; }
    }

    public class SubscriptionPlans : FulfillmentRequestResult
    {
        public IEnumerable<Plan> Plans { get; set; }
    }
}
