namespace SaaSFulfillmentClient.Models
{
    public class OperationUpdate
    {
        public string PlanId { get; set; }

        public int Quantity { get; set; }

        public OperationUpdateStatusEnum Status { get; set; }
    }
}
