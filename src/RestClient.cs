namespace SaaSFulfillmentClient
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using SaaSFulfillmentClient.AzureAD;

    public class RestClient<T>
    {
        protected const string DefaultApiVersionParameterName = "api-version";
        protected readonly string apiVersion;
        protected readonly string baseUri;
        protected readonly HttpMessageHandler httpMessageHandler;
        protected readonly ILogger<T> logger;
        protected readonly SecuredFulfillmentClientConfiguration options;

        protected RestClient(
            SecuredFulfillmentClientConfiguration securedFulfillmentClientConfiguration,
            ILogger<T> instanceLogger,
            HttpMessageHandler messageHandler)
        {
            this.options = securedFulfillmentClientConfiguration;
            this.logger = instanceLogger;
            this.baseUri = securedFulfillmentClientConfiguration.FulfillmentService.BaseUri;
            this.apiVersion = securedFulfillmentClientConfiguration.FulfillmentService.ApiVersion;
            this.httpMessageHandler = messageHandler;
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

        private string BuildReceivedLogMessage(
            Guid requestId,
            Guid correlationId,
            HttpStatusCode responseStatusCode,
            string result,
            string caller)
        {
            return
                $"Received response {caller}: requestId: {requestId} correlationId: {correlationId}. Status: {responseStatusCode}. Response content: {result}";
        }

        private string BuildSendLogMessage(Guid requestId, Guid correlationId, string caller, HttpMethod method, Uri requestUri, string content)
        {
            return $"Sending request {caller}, method:{method}, requestUri: {requestUri} requestId: {requestId} correlationId: {correlationId}, content:{content}";
        }

        private HttpClient GetHttpClient()
        {
            return this.httpMessageHandler == null ? new HttpClient() : new HttpClient(this.httpMessageHandler);
        }

#pragma warning disable CA1068 // CancellationToken parameters must come last

        protected async Task<HttpResponseMessage> SendRequestAndReturnResult(
#pragma warning restore CA1068 // CancellationToken parameters must come last
            HttpMethod method,
            Uri requestUri,
            Guid requestId,
            Guid correlationId,
            Action<HttpRequestMessage> customRequestBuilder = null,
            string content = "",
            CancellationToken cancellationToken = default,
            [CallerMemberName] string caller = "")
        {
            var bearerToken = await AdApplicationHelper.GetBearerToken(this.options);

            this.logger.LogInformation(this.BuildSendLogMessage(requestId, correlationId, caller, method, requestUri, content));
            using (var httpClient = this.GetHttpClient())
            {
                var marketplaceApiRequest =
                    BuildRequest(method, requestUri, requestId, correlationId, bearerToken, content);

                // Give option to modify the request for non-default settings
                customRequestBuilder?.Invoke(marketplaceApiRequest);

                try
                {
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
                }
                catch (WebException webException)
                {
                    this.logger.LogError(webException, $"Error received for request {marketplaceApiRequest.RequestUri.ToString()}");
                    throw;
                }

                var responseLogErrorMessage = $"Unsuccessful request ${marketplaceApiRequest.RequestUri}";
                this.logger.LogError(responseLogErrorMessage);
                throw new ApplicationException(responseLogErrorMessage);
            }
        }
    }
}
