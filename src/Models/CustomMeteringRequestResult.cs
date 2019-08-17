namespace SaaSFulfillmentClient.Models
{
    using System.Net;

    public class CustomMeteringRequestResult : AzureMarketplaceRequestResult
    {
        public CustomMeteringRequestResult()
        {
            this.Code = "Ok";
        }

        public string Code { get; set; }
    }
}
