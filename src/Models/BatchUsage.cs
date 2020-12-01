namespace SaaSFulfillmentClient.Models
{
    using System.Collections.Generic;

    public class BatchUsage
    {
        public IEnumerable<Usage> Request { get; set; }
    }
}
