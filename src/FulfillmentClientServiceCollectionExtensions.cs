using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SaaSFulfillmentClient.AzureAD;
using SaaSFulfillmentClient.WebHook;

namespace SaaSFulfillmentClient
{
    public static class FulfillmentClientServiceCollectionExtensions
    {
        public static void AddFulfillmentClient(this IServiceCollection services,
            Action<SecuredFulfillmentClientConfiguration> configureOptions,
            Action<FulfillmentClientBuilder> credentialBuilder)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services
                .AddOptions<SecuredFulfillmentClientConfiguration>()
                .Configure(configureOptions);

            credentialBuilder(new FulfillmentClientBuilder(services));
            services.TryAddScoped<IFulfillmentClient, FulfillmentClient>();
        }

        public static IServiceCollection AddWebhookProcessor(this IServiceCollection services)
        {
            services.TryAddScoped<IWebhookProcessor, WebhookProcessor>();
            return services;
        }

        public static void WithWebhookHandler<T>(this IServiceCollection services) where T : class, IWebhookHandler
        {
            services.TryAddScoped<IWebhookHandler, T>();
        }
    }
}
