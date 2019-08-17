namespace SaaSFulfillmentClient.Models
{
    using System.Collections.Generic;

    public class CustomerMeterErrorDetails
    {
        public string Message { get; set; }

        public string Target { get; set; }
    }

    public class CustomMeteringBadRequestResult : CustomMeteringRequestResult
    {
        public string Code { get; set; }
        public IEnumerable<CustomerMeterErrorDetails> Details { get; set; }
        public string Message { get; set; }

        public string Target { get; set; }
    }
}
