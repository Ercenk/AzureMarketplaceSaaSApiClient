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
        Unsubscribed,
        NotStarted,
        PendingFulfillmentStart
    }

    public class BeneficiaryOrPurchaser
    {
        public Guid TenantId { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("objectId")]
        public Guid ObjectId { get; set; }

        [JsonProperty("puid")]
        public string Puid { get; set; }
    }

    public class Subscription : FulfillmentRequestResult
    {
        public IEnumerable<AllowedCustomerOperationEnum> AllowedCustomerOperations { get; set; }
        public BeneficiaryOrPurchaser Beneficiary { get; set; }

        /// <summary>
        /// true – the customer subscription is currently in free trial, false – the customer subscription is not currently in free trial.
        /// </summary>
        public bool IsFreeTrial { get; set; }

        public string Name { get; set; }

        public string OfferId { get; set; }

        public string PlanId { get; set; }

        public string PublisherId { get; set; }

        public BeneficiaryOrPurchaser Purchaser { get; set; }

        public int Quantity { get; set; }

        public StatusEnum SaasSubscriptionStatus { get; set; }

        public SessionModeEnum SessionMode { get; set; }

        [JsonProperty("id")]
        public Guid SubscriptionId { get; set; }

        [JsonProperty("term")]
        public Term Term { get; set; }

        [JsonProperty("sessionId")]
        public Guid SessionId { get; set; }

        [JsonProperty("fulfillmentId")]
        public Guid FulfillmentId { get; set; }

        [JsonProperty("storeFront")]
        public string StoreFront { get; set; }
    }

    public class SubscriptionResult : FulfillmentRequestResult
    {
        public string ContinuationToken { get; set; }

        [JsonProperty("@nextLink")]
        public string NextLink { get; set; }
        public IList<Subscription> Subscriptions { get; set; }
    }
}
