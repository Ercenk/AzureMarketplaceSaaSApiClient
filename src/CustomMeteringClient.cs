namespace SaaSFulfillmentClient
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Newtonsoft.Json;

    using SaaSFulfillmentClient.AzureAD;
    using SaaSFulfillmentClient.Models;

    public class CustomMeteringClient : RestClient<CustomMeteringClient>, ICustomMeteringClient
    {
        public CustomMeteringClient(IOptionsMonitor<SecuredFulfillmentClientConfiguration> optionsMonitor,
                                 ILogger<CustomMeteringClient> logger) : this(null,
            optionsMonitor.CurrentValue,
            logger)
        {
        }

        public CustomMeteringClient(SecuredFulfillmentClientConfiguration options,
                                 ILogger<CustomMeteringClient> logger) : this(null,
            options,
            logger)
        {
        }

        public CustomMeteringClient(
            HttpMessageHandler httpMessageHandler,
            SecuredFulfillmentClientConfiguration options,
            ILogger<CustomMeteringClient> logger) : base(options, logger, httpMessageHandler)
        {
        }

        public async Task<CustomMeteringRequestResult> RecordBatchUsageAsync(Guid requestId, Guid correlationId, IEnumerable<Usage> usage, CancellationToken cancellationToken)
        {
            var requestUrl = FluentUriBuilder
                .Start(this.baseUri)
                .AddPath("batchUsageEvent")
                .AddQuery(DefaultApiVersionParameterName, this.apiVersion)
                .Uri;

            requestId = requestId == default ? Guid.NewGuid() : requestId;
            correlationId = correlationId == default ? Guid.NewGuid() : correlationId;

            var response = await this.SendRequestAndReturnResult(
                               HttpMethod.Post,
                               requestUrl,
                               requestId,
                               correlationId,
                               null,
                               JsonConvert.SerializeObject(usage),
                               cancellationToken);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return await AzureMarketplaceRequestResult.ParseAsync<CustomMeteringBatchSuccessResult>(response);

                case HttpStatusCode.Forbidden:
                    return await AzureMarketplaceRequestResult.ParseAsync<CustomMeteringForbiddenResult>(response);

                case HttpStatusCode.Conflict:
                    return await AzureMarketplaceRequestResult.ParseAsync<CustomMeteringConflictResult>(response);

                case HttpStatusCode.BadRequest:
                    return await AzureMarketplaceRequestResult.ParseAsync<CustomMeteringBadRequestResult>(response);

                default:
                    throw new ApplicationException($"Unknown response from the API {await response.Content.ReadAsStringAsync()}");
            }
        }

        public async Task<CustomMeteringRequestResult> RecordUsageAsync(Guid requestId, Guid correlationId, Usage usage, CancellationToken cancellationToken)
        {
            var requestUrl = FluentUriBuilder
                .Start(this.baseUri)
                .AddPath("usageEvent")
                .AddQuery(DefaultApiVersionParameterName, this.apiVersion)
                .Uri;

            requestId = requestId == default ? Guid.NewGuid() : requestId;
            correlationId = correlationId == default ? Guid.NewGuid() : correlationId;

            var response = await this.SendRequestAndReturnResult(
                               HttpMethod.Post,
                               requestUrl,
                               requestId,
                               correlationId,
                               null,
                               JsonConvert.SerializeObject(usage),
                               cancellationToken);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return await AzureMarketplaceRequestResult.ParseAsync<CustomMeteringSuccessResult>(response);

                case HttpStatusCode.Forbidden:
                    return await AzureMarketplaceRequestResult.ParseAsync<CustomMeteringForbiddenResult>(response);

                case HttpStatusCode.Conflict:
                    return await AzureMarketplaceRequestResult.ParseAsync<CustomMeteringConflictResult>(response);

                case HttpStatusCode.BadRequest:
                    return await AzureMarketplaceRequestResult.ParseAsync<CustomMeteringBadRequestResult>(response);

                default:
                    throw new ApplicationException($"Unknown response from the API {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}
