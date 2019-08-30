using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SaaSFulfillmentClient;
using SaaSFulfillmentClient.WebHook;
using Xunit;

namespace SaaSFulfillmentClientTests
{
    public class MockWebhookHandler : IWebhookHandler
    {
        public async Task ChangePlanAsync(WebhookPayload payload)
        {
            await Task.CompletedTask;

            Assert.True(true);
        }

        public async Task ChangeQuantityAsync(WebhookPayload payload)
        {
            await Task.CompletedTask;

            Assert.True(true);
        }

        public async Task ReinstatedAsync(WebhookPayload payload)
        {
            await Task.CompletedTask;

            Assert.True(true);
        }

        public async Task SubscribedAsync(WebhookPayload payload)
        {
            await Task.CompletedTask;

            Assert.True(true);
        }

        public async Task SuspendedAsync(WebhookPayload payload)
        {
            await Task.CompletedTask;

            Assert.True(true);
        }

        public async Task UnsubscribedAsync(WebhookPayload payload)
        {
            await Task.CompletedTask;

            Assert.True(true);
        }
    }

    public class WebHookTests
    {
        public WebHookTests()
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

            services.AddFulfillmentClient(options => configuration.Bind("FulfillmentClient", options));
            services
                .AddWebhookProcessor()
                .WithWebhookHandler<MockWebhookHandler>();

            this.serviceProvider = services.BuildServiceProvider();
        }

        private const string MockApiVersion = "2018-09-15";
        private const string MockUri = "https://marketplaceapi.microsoft.com/api/saas";
        private readonly ServiceProvider serviceProvider;

        [Fact]
        public async Task CanProcessWebhookRequest()
        {
            var processor = this.serviceProvider.GetRequiredService<IWebhookProcessor>();

            var webHookPayloadJson = @"{
                  ""id"": ""74dfb4db-c193-4891-827d-eb05fbdc64b0"",
                  ""activityId"": ""6CC5EA41-B96F-4E56-BDBC-D06511A909B2"",
                  ""subscriptionId"": ""37f9dea2-4345-438f-b0bd-03d40d28c7e0"",
                  ""publisherId"": ""Fabrikam"",
                  ""offerId"": ""<this is the offer name>"",
                  ""planId"": ""Platinum001"",
                  ""quantity"": ""20"",
                  ""timeStamp"": ""2019-04-15T20:17:31.7350641Z"",
                  ""action"": ""Unsubscribe"",
                  ""status"": ""NotStarted""
                }";

            var webHookPayload = JsonConvert.DeserializeObject<WebhookPayload>(webHookPayloadJson);
            await processor.ProcessWebhookNotificationAsync(webHookPayload);
        }
    }
}
