namespace SaaSFulfillmentClient.AzureTable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Azure.Cosmos.Table.Queryable;

    using SaaSFulfillmentClient.Models;

    public class AzureTableOperationsStore : IOperationsStore
    {
        private const string TableName = "maerketplaceoperations";
        private readonly CloudTableClient tableClient;

        public AzureTableOperationsStore(string storageAccountConnectionString)
        {
            this.tableClient = CloudStorageAccount.Parse(storageAccountConnectionString).CreateCloudTableClient();
        }

        public async Task<IEnumerable<OperationRecord>> GetAllSubscriptionRecordsAsync(Guid subscriptionId, CancellationToken cancellationToken = default)
        {
            var table = this.tableClient.GetTableReference(TableName);
            var result = new List<OperationRecord>();

            if (!table.Exists())
            {
                return result;
            }

            var query = new TableQuery().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, subscriptionId.ToString()));

            TableContinuationToken token = null;

            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync<OperationRecord>(
                                  query,
                                  (key, rowKey, timestamp, properties, etag) => new OperationRecord(key, rowKey),
                                  token,
                                  cancellationToken);

                result.AddRange(segment.Results.Select(r => r as OperationRecord));

                token = segment.ContinuationToken;
            }
            while (token != default);

            return result;
        }

        public async Task RecordAsync(
            Guid subscriptionId,
            UpdateOrDeleteSubscriptionRequestResult result,
            CancellationToken cancellationToken = default)
        {
            var table = this.tableClient.GetTableReference(TableName);
            await table.CreateIfNotExistsAsync(cancellationToken);

            var entity = new OperationRecord(subscriptionId.ToString(), result.OperationId.ToString());

            var tableOperation = TableOperation.InsertOrMerge(entity);

            await table.ExecuteAsync(tableOperation, cancellationToken);
        }
    }
}
