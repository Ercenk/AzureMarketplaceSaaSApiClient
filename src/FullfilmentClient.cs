using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using SaaSFulfillmentClient.AzureAD;
using SaaSFulfillmentClient.Models;

namespace SaaSFulfillmentClient
{
    public class FulfillmentClient : IFulfillmentClient
    {
        private const string DefaultApiVersionParameterName = "api-version";
        private readonly string apiVersion;
        private readonly string baseUri;
        private readonly HttpMessageHandler httpMessageHandler;
        private readonly ILogger<FulfillmentClient> logger;

        private readonly SecuredFulfillmentClientConfiguration options;

        public FulfillmentClient(IOptionsMonitor<SecuredFulfillmentClientConfiguration> optionsMonitor,
            ICredentialProvider credentialProvider,
            ILogger<FulfillmentClient> logger) : this(null,
            optionsMonitor.CurrentValue,
            credentialProvider,
            AdApplicationHelper.GetApplication,
            logger)
        {
        }

        public FulfillmentClient(SecuredFulfillmentClientConfiguration options,
            ICredentialProvider credentialProvider,
            ILogger<FulfillmentClient> logger) : this(null,
            options,
            credentialProvider,
            AdApplicationHelper.GetApplication,
            logger)
        {
        }

        public FulfillmentClient(
            HttpMessageHandler httpMessageHandler,
            SecuredFulfillmentClientConfiguration options,
            ICredentialProvider credentialProvider,
            Func<SecuredFulfillmentClientConfiguration, ICredentialProvider, IConfidentialClientApplication>
                adApplicationFactory,
            ILogger<FulfillmentClient> logger)
        {
            this.options = options;
            this.logger = logger;
            this.AdApplication = adApplicationFactory(this.options, credentialProvider);
            this.baseUri = options.FulfillmentService.BaseUri;
            this.apiVersion = options.FulfillmentService.ApiVersion;
            this.logger = logger;
            this.httpMessageHandler = httpMessageHandler;
        }

        private readonly IConfidentialClientApplication AdApplication;

        public async Task<FulfillmentRequestResult> ActivateSubscriptionAsync(Guid subscriptionId,
            ActivatedSubscription subscriptionDetails, Guid requestId, Guid correlationId,
            CancellationToken cancellationToken)
        {
            var bearerToken = await AdApplicationHelper.GetBearerToken(this.AdApplication);
            var requestUrl = FluentUriBuilder
                .Start(this.baseUri)
                .AddPath("subscriptions")
                .AddPath(subscriptionId.ToString())
                .AddPath("activate")
                .AddQuery(DefaultApiVersionParameterName, this.apiVersion)
                .Uri;

            requestId = requestId == default ? Guid.NewGuid() : requestId;
            correlationId = correlationId == default ? Guid.NewGuid() : correlationId;

            var response = await this.SendRequestAndReturnResult(
                HttpMethod.Post,
                requestUrl,
                requestId,
                correlationId,
                bearerToken,
                null,
                JsonConvert.SerializeObject(subscriptionDetails),
                cancellationToken);

            return await FulfillmentRequestResult.ParseAsync<FulfillmentRequestResult>(response);
        }

        public async Task<UpdateOrDeleteSubscriptionRequestResult> DeleteSubscriptionAsync(Guid subscriptionId,
            Guid requestId, Guid correlationId, CancellationToken cancellationToken)
        {
            var bearerToken = await AdApplicationHelper.GetBearerToken(this.AdApplication);
            var requestUrl = FluentUriBuilder
                .Start(this.baseUri)
                .AddPath("subscriptions")
                .AddPath(subscriptionId.ToString())
                .AddQuery(DefaultApiVersionParameterName, this.apiVersion)
                .Uri;

            requestId = requestId == default ? Guid.NewGuid() : requestId;
            correlationId = correlationId == default ? Guid.NewGuid() : correlationId;

            var response = await this.SendRequestAndReturnResult(
                HttpMethod.Delete,
                requestUrl,
                requestId,
                correlationId,
                bearerToken,
                null,
                "",
                cancellationToken);

            return await FulfillmentRequestResult.ParseAsync<UpdateOrDeleteSubscriptionRequestResult>(response);
        }

        public async Task<IEnumerable<SubscriptionOperation>> GetOperationsAsync(Guid requestId, Guid correlationId,
            CancellationToken cancellationToken)
        {
            var bearerToken = await AdApplicationHelper.GetBearerToken(this.AdApplication);
            var requestUrl = FluentUriBuilder
                .Start(this.baseUri)
                .AddPath("operations")
                .AddQuery(DefaultApiVersionParameterName, this.apiVersion)
                .Uri;

            requestId = requestId == default ? Guid.NewGuid() : requestId;
            correlationId = correlationId == default ? Guid.NewGuid() : correlationId;

            var response = await this.SendRequestAndReturnResult(HttpMethod.Get,
                requestUrl,
                requestId,
                correlationId,
                bearerToken,
                null,
                "",
                cancellationToken);

            return await FulfillmentRequestResult.ParseMultipleAsync<SubscriptionOperation>(response);
        }

        public async Task<Subscription> GetSubscriptionAsync(Guid subscriptionId, Guid requestId, Guid correlationId,
            CancellationToken cancellationToken)
        {
            var bearerToken = await AdApplicationHelper.GetBearerToken(this.AdApplication);
            var requestUrl = FluentUriBuilder
                .Start(this.baseUri)
                .AddPath("subscriptions")
                .AddPath(subscriptionId.ToString())
                .AddQuery(DefaultApiVersionParameterName, this.apiVersion)
                .Uri;

            requestId = requestId == default ? Guid.NewGuid() : requestId;
            correlationId = correlationId == default ? Guid.NewGuid() : correlationId;

            var response = await this.SendRequestAndReturnResult(HttpMethod.Get,
                requestUrl,
                requestId,
                correlationId,
                bearerToken,
                null,
                "",
                cancellationToken);

            return await FulfillmentRequestResult.ParseAsync<Subscription>(response);
        }

        public async Task<SubscriptionOperation> GetSubscriptionOperationAsync(Guid subscriptionId, Guid operationId,
            Guid requestId, Guid correlationId, CancellationToken cancellationToken)
        {
            var bearerToken = await AdApplicationHelper.GetBearerToken(this.AdApplication);
            var requestUrl = FluentUriBuilder
                .Start(this.baseUri)
                .AddPath("subscriptions")
                .AddPath(subscriptionId.ToString())
                .AddPath("operations")
                .AddPath(operationId.ToString())
                .AddQuery(DefaultApiVersionParameterName, this.apiVersion)
                .Uri;

            requestId = requestId == default ? Guid.NewGuid() : requestId;
            correlationId = correlationId == default ? Guid.NewGuid() : correlationId;

            var response = await this.SendRequestAndReturnResult(
                HttpMethod.Get,
                requestUrl,
                requestId,
                correlationId,
                bearerToken,
                null,
                "",
                cancellationToken);

            return await FulfillmentRequestResult.ParseAsync<SubscriptionOperation>(response);
        }

        public async Task<IEnumerable<SubscriptionOperation>> GetSubscriptionOperationsAsync(Guid subscriptionId,
            Guid requestId, Guid correlationId, CancellationToken cancellationToken)
        {
            var bearerToken = await AdApplicationHelper.GetBearerToken(this.AdApplication);
            var requestUrl = FluentUriBuilder
                .Start(this.baseUri)
                .AddPath("subscriptions")
                .AddPath(subscriptionId.ToString())
                .AddPath("operations")
                .AddQuery(DefaultApiVersionParameterName, this.apiVersion)
                .Uri;

            requestId = requestId == default ? Guid.NewGuid() : requestId;
            correlationId = correlationId == default ? Guid.NewGuid() : correlationId;

            var response = await this.SendRequestAndReturnResult(HttpMethod.Get,
                requestUrl,
                requestId,
                correlationId,
                bearerToken,
                null,
                "",
                cancellationToken);

            return await FulfillmentRequestResult.ParseMultipleAsync<SubscriptionOperation>(response);
        }

        public async Task<SubscriptionPlans> GetSubscriptionPlansAsync(Guid subscriptionId, Guid requestId,
            Guid correlationId, CancellationToken cancellationToken)
        {
            var bearerToken = await AdApplicationHelper.GetBearerToken(this.AdApplication);
            var requestUrl = FluentUriBuilder
                .Start(this.baseUri)
                .AddPath("subscriptions")
                .AddPath(subscriptionId.ToString())
                .AddPath("listAvailablePlans")
                .AddQuery(DefaultApiVersionParameterName, this.apiVersion)
                .Uri;

            requestId = requestId == default ? Guid.NewGuid() : requestId;
            correlationId = correlationId == default ? Guid.NewGuid() : correlationId;

            var response = await this.SendRequestAndReturnResult(HttpMethod.Get,
                requestUrl,
                requestId,
                correlationId,
                bearerToken,
                null,
                "",
                cancellationToken);

            return await FulfillmentRequestResult.ParseAsync<SubscriptionPlans>(response);
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync(
            Guid requestId,
            Guid correlationId,
            CancellationToken cancellationToken)
        {
            var bearerToken = await AdApplicationHelper.GetBearerToken(this.AdApplication);
            var requestUrl = FluentUriBuilder
                .Start(this.baseUri)
                .AddPath("subscriptions")
                .AddQuery(DefaultApiVersionParameterName, this.apiVersion)
                .Uri;

            requestId = requestId == default ? Guid.NewGuid() : requestId;
            correlationId = correlationId == default ? Guid.NewGuid() : correlationId;

            var response = await this.SendRequestAndReturnResult(HttpMethod.Get,
                requestUrl,
                requestId,
                correlationId,
                bearerToken,
                null,
                "",
                cancellationToken);

            return await FulfillmentRequestResult.ParseMultipleAsync<Subscription>(response);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="marketplaceToken">
        /// Token query parameter in the URL when the user is redirected to the SaaS ISV’s website from Azure.
        /// Note: The URL decodes the token value from the browser before using it.
        /// This token is valid only for 1 hour
        /// </param>
        /// <param name="requestId"></param>
        /// <param name="correlationId"></param>
        /// <param name="bearerToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResolvedSubscription> ResolveSubscriptionAsync(string marketplaceToken, Guid requestId,
            Guid correlationId, CancellationToken cancellationToken)
        {
            var bearerToken = await AdApplicationHelper.GetBearerToken(this.AdApplication);
            var requestUrl = FluentUriBuilder
                .Start(this.baseUri)
                .AddPath("subscriptions")
                .AddPath("resolve")
                .AddQuery(DefaultApiVersionParameterName, this.apiVersion)
                .Uri;

            requestId = requestId == default ? Guid.NewGuid() : requestId;
            correlationId = correlationId == default ? Guid.NewGuid() : correlationId;

            var response = await this.SendRequestAndReturnResult(HttpMethod.Post,
                requestUrl,
                requestId,
                correlationId,
                bearerToken,
                r =>
                {
                    r.Headers.Add("x-ms-marketplace-token", marketplaceToken);
                },
                "",
                cancellationToken);

            return await FulfillmentRequestResult.ParseAsync<ResolvedSubscription>(response);
        }

        public async Task<UpdateOrDeleteSubscriptionRequestResult> UpdateSubscriptionAsync(Guid subscriptionId,
            ActivatedSubscription update, Guid requestId, Guid correlationId, CancellationToken cancellationToken)
        {
            var bearerToken = await AdApplicationHelper.GetBearerToken(this.AdApplication);
            var requestUrl = FluentUriBuilder
                .Start(this.baseUri)
                .AddPath("subscriptions")
                .AddPath(subscriptionId.ToString())
                .AddQuery(DefaultApiVersionParameterName, this.apiVersion)
                .Uri;

            requestId = requestId == default ? Guid.NewGuid() : requestId;
            correlationId = correlationId == default ? Guid.NewGuid() : correlationId;
            var updateContent = JsonConvert.SerializeObject(update);

            if (!string.IsNullOrEmpty(update.PlanId) && !string.IsNullOrEmpty(update.Quantity))
            {
                throw new ApplicationException("Plan Id and quantity cannot be patched at the same time.");
            }

            var response = await this.SendRequestAndReturnResult(
                new HttpMethod("PATCH"),
                requestUrl,
                requestId,
                correlationId,
                bearerToken,
                null,
                updateContent,
                cancellationToken);

            return await FulfillmentRequestResult.ParseAsync<UpdateOrDeleteSubscriptionRequestResult>(response);
        }

        private static HttpRequestMessage BuildRequest(
            HttpMethod method,
            Uri requestUri,
            Guid requestId,
            Guid correlationId,
            string bearerToken,
            string content)
        {
            var request = new HttpRequestMessage { RequestUri = requestUri, Method = method };

            request.Headers.Add("x-ms-requestid", requestId.ToString());
            request.Headers.Add("x-ms-correlationid", correlationId.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            if (method == HttpMethod.Post ||
                method.ToString().ToUpper() == "PATCH")
            {
                request.Content = new StringContent(content);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return request;
        }

        private string BuildReceivedLogMessage(Guid requestId, Guid correlationId, HttpStatusCode responseStatusCode,
            string result, string caller)
        {
            return
                $"Received response {caller}: requestId: {requestId} correlationId: {correlationId}. Status: {responseStatusCode}. Response content: {result}";
        }

        private string BuildSendLogMessage(Guid requestId, Guid correlationId, string caller)
        {
            return $"Sending request {caller}: requestId: {requestId} correlationId: {correlationId}";
        }

        private HttpClient GetHttpClient()
        {
            if (this.httpMessageHandler == null)
            {
                return new HttpClient();
            }

            return new HttpClient(this.httpMessageHandler);
        }

#pragma warning disable CA1068 // CancellationToken parameters must come last

        private async Task<HttpResponseMessage> SendRequestAndReturnResult(
#pragma warning restore CA1068 // CancellationToken parameters must come last
            HttpMethod method,
            Uri requestUri,
            Guid requestId,
            Guid correlationId,
            string bearerToken,
            Action<HttpRequestMessage> customRequestBuilder = null,
            string content = "",
            CancellationToken cancellationToken = default,
            [CallerMemberName] string caller = "")
        {
            this.logger.LogInformation(this.BuildSendLogMessage(requestId, correlationId, caller));
            using (var httpClient = this.GetHttpClient())
            {
                var marketplaceApiRequest =
                    BuildRequest(method, requestUri, requestId, correlationId, bearerToken, content);

                // Give option to modify the request for non-default settings
                customRequestBuilder?.Invoke(marketplaceApiRequest);

                var response = await httpClient.SendAsync(marketplaceApiRequest, cancellationToken);
                var result = await response.Content.ReadAsStringAsync();
                var responseLogMessage = this.BuildReceivedLogMessage(requestId, correlationId, response.StatusCode,
                    result,
                    caller);

                if (response.IsSuccessStatusCode)
                {
                    this.logger.LogInformation(responseLogMessage);

                    return response;
                }

                this.logger.LogError(responseLogMessage);
                throw new ApplicationException(responseLogMessage);
            }
        }
    }
}
