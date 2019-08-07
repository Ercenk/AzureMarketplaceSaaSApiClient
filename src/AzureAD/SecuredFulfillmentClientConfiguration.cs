using SaaSFulfillmentClient.Models;

namespace SaaSFulfillmentClient.AzureAD
{
    public class SecuredFulfillmentClientConfiguration
    {
        public AuthenticationConfiguration AzureActiveDirectory { get; set; }

        public FulfillmentClientConfiguration FulfillmentService { get; set; }
    }
}
