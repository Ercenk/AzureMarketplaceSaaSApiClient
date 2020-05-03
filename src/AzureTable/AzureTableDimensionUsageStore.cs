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

    public class AzureTableDimensionUsageStore : IDimensionStore
    {
        private const string TableName = "marketplacedimensionusage";
        private readonly CloudTableClient tableClient;

        public AzureTableDimensionUsageStore(string storageAccountConnectionString)
        {
            this.tableClient = CloudStorageAccount.Parse(storageAccountConnectionString).CreateCloudTableClient();
        }

        public async Task<IEnumerable<DimensionUsageRecord>> GetAllDimensionRecordsAsync(Guid subscriptionId, CancellationToken cancellationToken = default)
        {
            var table = this.tableClient.GetTableReference(TableName);
            var result = new List<DimensionUsageRecord>();

            if (!table.Exists())
            {
                return result;
            }

            var query = new TableQuery().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, subscriptionId.ToString()));

            TableContinuationToken token = null;

            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync<DimensionUsageRecord>(
                                  query,
                                  (key, rowKey, timestamp, properties, etag) => new DimensionUsageRecord(key, rowKey),
                                  token,
                                  cancellationToken);

                result.AddRange(segment.Results.Select(r => r as DimensionUsageRecord));

                token = segment.ContinuationToken;
            }
            while (token != default);

            return result;
        }

        public async Task RecordAsync(string subscriptionId, CustomMeteringRequestResult result, CancellationToken cancellationToken = default)
        {
            var table = this.tableClient.GetTableReference(TableName);

            await table.CreateIfNotExistsAsync(cancellationToken);

            var entity = new DimensionUsageRecord(result.RequestResourceId, result.RequestSentTime.ToString())
            {
                RequestResourceId = result.RequestResourceId,
                RequestOfferId = result.RequestOfferId,
                RequestPlanId = result.RequestPlanId,
                RequestDimensionId = result.RequestDimensionId,
                RequestQuantity = result.RequestQuantity,
                RequestSentTime = result.RequestSentTime,
                RequestUsageTime = result.RequestUsageTime,
                UsageEventId = result.UsageEventId,
                Code = result.Code,
                StatusCode = result.StatusCode,
                RawResponse = result.RawResponse,
            }; 

            var tableOperation = TableOperation.InsertOrMerge(entity);

            await table.ExecuteAsync(tableOperation, cancellationToken);
        }
    }
}
