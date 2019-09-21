namespace SaaSFulfillmentClient.Models
{
    using System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public enum TermUnitEnum
    {
        P1M,
        P1Y
    }

    public class Term
    {
        [JsonProperty("endDate")]
        public DateTimeOffset EndDate { get; set; }

        [JsonProperty("startDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty("TermUnit")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TermUnitEnum TermUnit { get; set; }
    }
}
