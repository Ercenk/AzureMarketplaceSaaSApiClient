using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaaSFulfillmentClient.Models
{
    public enum AllowedCustomerOperationEnum
    {
        Read,
        Update,
        Delete
    }

    public enum SessionModeEnum
    {
        None,
        DryRun
    }

    public enum StatusEnum
    {
        Provisioning,
        Subscribed,
        Suspended,
        Unsubscribed
    }

    public class Beneficiary
    {
        public Guid TenantId { get; set; }
    }

    public class Purchaser
    {
        public Guid TenantId { get; set; }
    }

    public class Subscription : FulfillmentRequestResult
    {
        public IEnumerable<AllowedCustomerOperationEnum> AllowedCustomerOperations { get; set; }
        public Beneficiary Beneficiary { get; set; }

        public string Name { get; set; }

        public string OfferId { get; set; }

        public string PlanId { get; set; }

        public string PublisherId { get; set; }

        public Purchaser Purchaser { get; set; }

        public int Quantity { get; set; }

        public StatusEnum SaasSubscriptionStatus { get; set; }

        public SessionModeEnum SessionMode { get; set; }

        [JsonProperty("id")] public Guid SubscriptionId { get; set; }
    }
}
