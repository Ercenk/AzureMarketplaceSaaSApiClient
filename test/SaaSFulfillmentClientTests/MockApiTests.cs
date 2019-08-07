using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using SaaSFulfillmentClient;
using SaaSFulfillmentClient.Models;
using Xunit;

namespace SaaSFulfillmentClientTests
{
    public class MockApiTests
    {
        public MockApiTests()
        {
            this.loggerMock = new Mock<ILogger<FulfillmentClient>>();
            var configuration = new FulfillmentClientConfiguration {BaseUri = MockUri, ApiVersion = MockApiVersion};

            this.client = new FulfillmentClient(configuration, this.loggerMock.Object);
        }

        private const string MockApiVersion = "2018-09-15";
        private const string MockUri = "https://marketplaceapi.microsoft.com/api/saas";
        private readonly FulfillmentClient client;
        private readonly Mock<ILogger<FulfillmentClient>> loggerMock;

        [Fact]
        public async Task CanActivateSubscription()
        {
            var correlationId = Guid.NewGuid();
            var requestId = Guid.NewGuid();

            var subscriptions = await this.client.GetSubscriptionsAsync(
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.Equal(requestId, subscriptions.First().RequestId);

            requestId = Guid.NewGuid();

            var result = await this.client.ActivateSubscriptionAsync(
                subscriptions.First().SubscriptionId,
                new ActivatedSubscription {PlanId = "Gold", Quantity = ""},
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.Equal(requestId, result.RequestId);
        }

        [Fact]
        public async Task CanDeleteSubscription()
        {
            var correlationId = Guid.NewGuid();
            var requestId = Guid.NewGuid();

            var subscriptions = await this.client.GetSubscriptionsAsync(
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.Equal(requestId, subscriptions.First().RequestId);

            requestId = Guid.NewGuid();

            var result = await this.client.DeleteSubscriptionAsync(
                subscriptions.First().SubscriptionId,
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.IsType<Guid>(result.RequestId);
            Assert.NotEqual(Guid.Empty, result.RequestId);
            Assert.NotEqual(0, result.RetryAfter);
        }

        [Fact]
        public async Task CanGetAllnOperations()
        {
            var correlationId = Guid.NewGuid();
            var requestId = Guid.NewGuid();

            var subscriptions = await this.client.GetSubscriptionsAsync(
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.Equal(requestId, subscriptions.First().RequestId);

            requestId = Guid.NewGuid();

            var result = await this.client.GetOperationsAsync(
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task CanGetSubscription()
        {
            var correlationId = Guid.NewGuid();
            var requestId = Guid.NewGuid();

            var subscriptions = await this.client.GetSubscriptionsAsync(
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.Equal(requestId, subscriptions.First().RequestId);

            requestId = Guid.NewGuid();

            var result = await this.client.GetSubscriptionAsync(
                subscriptions.First().SubscriptionId,
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.NotNull(result);
            Assert.Equal(subscriptions.First().SubscriptionId, result.SubscriptionId);
        }

        [Fact]
        public async Task CanGetSubscriptionOperation()
        {
            var correlationId = Guid.NewGuid();
            var requestId = Guid.NewGuid();

            var subscriptions = await this.client.GetSubscriptionsAsync(
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.Equal(requestId, subscriptions.First().RequestId);

            requestId = Guid.NewGuid();

            var operations = await this.client.GetSubscriptionOperationsAsync(
                subscriptions.First().SubscriptionId,
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.Equal(requestId, operations.First().RequestId);

            var operation = await this.client.GetSubscriptionOperationAsync(
                subscriptions.First().SubscriptionId,
                operations.First().Id,
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.NotNull(operation);
            Assert.IsType<OperationStatusEnum>(operation.Status);
        }

        [Fact]
        public async Task CanGetSubscriptionOperations()
        {
            var correlationId = Guid.NewGuid();
            var requestId = Guid.NewGuid();

            var subscriptions = await this.client.GetSubscriptionsAsync(
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.Equal(requestId, subscriptions.First().RequestId);

            requestId = Guid.NewGuid();

            var result = await this.client.GetSubscriptionOperationsAsync(
                subscriptions.First().SubscriptionId,
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task CanGetSubscriptionPlans()
        {
            var correlationId = Guid.NewGuid();
            var requestId = Guid.NewGuid();

            var subscriptions = await this.client.GetSubscriptionsAsync(
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.Equal(requestId, subscriptions.First().RequestId);

            requestId = Guid.NewGuid();

            var result = await this.client.GetSubscriptionPlansAsync(
                subscriptions.First().SubscriptionId,
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.NotEmpty(result.Plans);
        }

        [Fact]
        public async Task CanGetSubscriptions()
        {
            var correlationId = Guid.NewGuid();
            var requestId = Guid.NewGuid();

            var subscriptions = await this.client.GetSubscriptionsAsync(
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.NotEmpty(subscriptions);
            Assert.Equal(requestId, subscriptions.First().RequestId);
        }

        [Fact]
        public async Task CanResolveSubscription()
        {
            var correlationId = Guid.NewGuid();
            var requestId = Guid.NewGuid();

            var result = await this.client.ResolveSubscriptionAsync(
                "marketplacetoken",
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);
            Assert.Equal(requestId, result.RequestId);

            Assert.NotNull(result);
            Assert.IsType<ResolvedSubscription>(result);
        }

        [Fact]
        public async Task CanUpdateSubscription()
        {
            var correlationId = Guid.NewGuid();
            var requestId = Guid.NewGuid();

            var subscriptions = await this.client.GetSubscriptionsAsync(
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.Equal(requestId, subscriptions.First().RequestId);

            requestId = Guid.NewGuid();

            var update = new ActivatedSubscription {PlanId = "Gold", Quantity = ""};

            var result = await this.client.UpdateSubscriptionAsync(
                subscriptions.First().SubscriptionId,
                update,
                requestId,
                correlationId,
                string.Empty,
                new CancellationTokenSource().Token);

            Assert.NotNull(result.Operation);
            Assert.NotNull(result);
            Assert.NotEqual(0, result.RetryAfter);
        }
    }
}
