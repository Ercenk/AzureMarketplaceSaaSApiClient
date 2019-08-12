using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SaaSFulfillmentClient;
using SaaSFulfillmentClient.AzureAD;
using SaaSFulfillmentClient.Models;
using Xunit;

namespace SaaSFulfillmentClientTests
{
    public class ClientTests
    {
        private const string MockApiVersion = "2018-09-15";

        private const string MockUri = "https://marketplaceapi.microsoft.com/api/saas";

        private readonly FulfillmentClient client;

        private readonly Mock<ILogger<FulfillmentClient>> loggerMock;

        private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;

        public ClientTests()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<ClientTests>();
            var configuration = builder.Build();

            this.mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            this.loggerMock = new Mock<ILogger<FulfillmentClient>>();
            var options = new SecuredFulfillmentClientConfiguration
            {
                FulfillmentService =
                    new FulfillmentClientConfiguration { BaseUri = MockUri, ApiVersion = MockApiVersion },
                AzureActiveDirectory = new AuthenticationConfiguration
                {
                    ClientId = "84aca647-1340-454b-923c-a21a9003b28e",
                    AppKey = configuration["FulfillmentClient:AzureActiveDirectory:AppKey"]
                }
            };

            var credentialProvider = new ClientSecretCredentialProvider(options.AzureActiveDirectory.AppKey);

            this.client = new FulfillmentClient(this.mockHttpMessageHandler.Object, options, credentialProvider,
                AdApplicationHelper.GetApplication, this.loggerMock.Object);
        }

        [Fact]
        public async Task CanBuildClientAndCall()
        {
            var configurationDictionary = new Dictionary<string, string>
            {
                {"FulfillmentClient:AzureActiveDirectory:ClientId", "84aca647-1340-454b-923c-a21a9003b28e"},
                {"FulfillmentClient:FulfillmentService:ApiVersion", MockApiVersion},
                {"FulfillmentClient:FulfillmentService:BaseUri", MockUri}
            };
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<ClientTests>()
                .AddInMemoryCollection(configurationDictionary)
                .Build();

            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddConsole());

            services.AddFulfillmentClient(options => configuration.Bind("FulfillmentClient", options),
                credentialBuilder =>
                    credentialBuilder.WithClientSecretAuthentication(
                        configuration["FulfillmentClient:AzureActiveDirectory:AppKey"]));

            var serviceProvider = services.BuildServiceProvider();

            var client = serviceProvider.GetRequiredService<IFulfillmentClient>();

            await this.CanGetAllSubscriptions();
        }

        [Fact]
        public async Task CanGetAllSubscriptions()
        {
            var requestId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();

            this.mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r =>
                        r.RequestUri.AbsoluteUri.Contains(
                            "https://marketplaceapi.microsoft.com/api/saas/subscriptions")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(GenerateSubscriptions(10)))
                })
                .Callback<HttpRequestMessage, CancellationToken>((r, c) =>
                {
                    var queryParameters = HttpUtility.ParseQueryString(r.RequestUri.Query);

                    // Only one query parameter on the outgoing request
                    Assert.Single(queryParameters);

                    // Is it the ApiVersion and the correct one?
                    Assert.Equal("api-version", queryParameters.Keys[0]);
                    Assert.Equal(MockApiVersion, queryParameters[0]);

                    // Check headers
                    var headers = r.Headers;

                    Assert.Equal(3, headers.Count());
                    Assert.True(headers.Contains("x-ms-requestid") &&
                                headers.Contains("x-ms-correlationid") &&
                                headers.Contains("authorization"));
                });

            var result = await this.client.GetSubscriptionsAsync(
                requestId,
                correlationId,
                new CancellationTokenSource().Token);

            Assert.IsAssignableFrom<IEnumerable<Subscription>>(result);
            Assert.Equal(10, result.ToList().Count());
        }

        [Fact]
        public async Task CatchExceptionWhen500ForGetAllSubscriptions()
        {
            var requestId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();

            var errorMessage = @"
                {
                    ""error"": {
                      ""code"": ""UnexpectedError"",
                      ""message"": ""An unexpected error has occurred.""
                    }
                }";

            this.mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r =>
                        r.RequestUri.AbsoluteUri.Contains(
                            "https://marketplaceapi.microsoft.com/api/saas/subscriptions")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(errorMessage)
                }).Callback<HttpRequestMessage, CancellationToken>((r, c) =>
                {
                    var queryParameters = HttpUtility.ParseQueryString(r.RequestUri.Query);

                    // Only one query parameter on the outgoing request
                    Assert.Single(queryParameters);

                    // Is it the ApiVersion and the correct one?
                    Assert.Equal("api-version", queryParameters.Keys[0]);
                    Assert.Equal(MockApiVersion, queryParameters[0]);

                    // Check headers
                    var headers = r.Headers;

                    Assert.Equal(3, headers.Count());
                    Assert.True(headers.Contains("x-ms-requestid") &&
                                headers.Contains("x-ms-correlationid") &&
                                headers.Contains("authorization"));
                });

            try
            {
                var result = await this.client.GetSubscriptionsAsync(
                    requestId,
                    correlationId,
                    new CancellationTokenSource().Token);
            }
            catch (ApplicationException exception)
            {
                Assert.Contains("Received response", exception.Message);
            }
        }

        private static IEnumerable<Subscription> GenerateSubscriptions(int numberOfSubscriptions)
        {
            return
                Enumerable.Range(0, numberOfSubscriptions).Select(r => new Subscription
                {
                    SubscriptionId = Guid.NewGuid(),
                    Name = $"subscription{r}",
                    OfferId = $"offer{r}",
                    PlanId = $"silver{r}",
                    Quantity = 10 + r,
                    Beneficiary = new Beneficiary { TenantId = Guid.NewGuid() },
                    Purchaser = new Purchaser { TenantId = Guid.NewGuid() },
                    AllowedCustomerOperations = new List<AllowedCustomerOperationEnum>
                    {
                        //Enum.GetValues(typeof(AllowedCustomerOperationEnum))
                        AllowedCustomerOperationEnum.Read, AllowedCustomerOperationEnum.Update
                    },
                    SessionMode = SessionModeEnum.None,
                    SaasSubscriptionStatus = StatusEnum.Provisioning
                }).ToList();
        }
    }
}
