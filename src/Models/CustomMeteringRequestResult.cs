namespace SaaSFulfillmentClient.Models
{
    using System;
    using System.Net;

    public class CustomMeteringRequestResult : AzureMarketplaceRequestResult
    {
        public CustomMeteringRequestResult()
        {
            this.Code = "Ok";
        }

        public string Code { get; set; }
        public string RequestResourceId { get; set; }
        public string RequestPlanId { get; set; }
        public string RequestDimensionId { get; set; }
        public Double RequestQuantity { get; set; }
        public string RequestSentTime { get; set; }
        public string RequestUsageTime { get; set; }

        public string UsageEventId { get; set; }
    }
}
