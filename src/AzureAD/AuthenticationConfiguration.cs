using System;

namespace SaaSFulfillmentClient.AzureAD
{
    public class AuthenticationConfiguration
    {
        public string AppKey { get; set; }

        public Guid ClientId { get; set; }

        public Guid TenantId { get; set; }
    }
}
