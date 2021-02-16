namespace SaaSFulfillmentClient.AzureTable
{
    using System;
    using System.Net;
    using Microsoft.Azure.Cosmos.Table;

    public class DimensionUsageRecord : TableEntity
    {
        public DimensionUsageRecord(string subscriptionId, string messageDateTime)
        {
            this.PartitionKey = subscriptionId;
            this.RowKey = messageDateTime;
        }

        public string MessageDateTime
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value.ToString();
            }
        }

        public Guid SubscriptionId
        {
            get
            {
                return Guid.Parse(this.PartitionKey);
            }

            set
            {
                this.PartitionKey = value.ToString();
            }
        }

        public string RequestResourceId { get; set; }        
        public string RequestOfferId { get; set; }
        public string RequestPlanId { get; set; }
        public string RequestDimensionId { get; set; }
        public double RequestQuantity { get; set;}
        public string RequestSentTime { get; set; }
        public string RequestUsageTime { get; set; }
        public string UsageEventId { get; set; }
        public string Code { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string RawResponse { get; set; }
}
}
