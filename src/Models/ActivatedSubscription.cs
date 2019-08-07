namespace SaaSFulfillmentClient.Models
{
    public class ActivatedSubscription : FulfillmentRequestResult
    {
        public string PlanId { get; set; }
        public string Quantity { get; set; }
    }
}
