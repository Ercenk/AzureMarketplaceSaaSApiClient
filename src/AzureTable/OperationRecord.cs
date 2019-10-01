namespace SaaSFulfillmentClient.AzureTable
{
    using System;

    using Microsoft.Azure.Cosmos.Table;

    public class OperationRecord : TableEntity
    {
        public OperationRecord(string subscriptionId, string operationId)
        {
            this.PartitionKey = subscriptionId;
            this.RowKey = operationId;
        }

        public Guid OperationId
        {
            get
            {
                return Guid.Parse(this.RowKey);
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
    }
}
