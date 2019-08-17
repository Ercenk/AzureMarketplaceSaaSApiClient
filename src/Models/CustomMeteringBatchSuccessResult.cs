namespace SaaSFulfillmentClient
{
    using System.Collections.Generic;

    using SaaSFulfillmentClient.Models;

    public class CustomMeteringBatchSuccessResult : CustomMeteringRequestResult
    {
        public int Count { get; set; }

        public IEnumerable<CustomMeteringSuccessResult> Result { get; set; }
    }
}
