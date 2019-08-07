using System;
using System.Linq;
using System.Net.Http.Headers;

namespace SaaSFulfillmentClient.Models
{
    public class UpdateOrDeleteSubscriptionRequestResult : FulfillmentRequestResult
    {
        private const string OperationLocationKey = "Operation-Location";
        private const string RetryAfterKey = "Retry-After";

        public Uri Operation { get; set; }

        public int RetryAfter { get; set; }

        protected override void UpdateFromHeaders(HttpHeaders headers)
        {
            base.UpdateFromHeaders(headers);

            if (headers.TryGetValues(OperationLocationKey, out var values))
            {
                Uri.TryCreate(values.First(), UriKind.Absolute, out var operationUri);
                this.Operation = operationUri ?? throw new ApplicationException("API did not return an operation ID");
            }

            if (headers.TryGetValues(RetryAfterKey, out values))
            {
                int.TryParse(values.First(), out var retryAfter);

                if (retryAfter == 0)
                {
                    throw new ApplicationException("API did not return a retry-after value");
                }

                this.RetryAfter = retryAfter;
            }
        }
    }
}
