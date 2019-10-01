using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SaaSFulfillmentClient.AzureAD;
using SaaSFulfillmentClient.WebHook;

namespace SaaSFulfillmentClient
{
    using SaaSFulfillmentClient.AzureTable;

    public static class FulfillmentClientServiceCollectionExtensions
    {
        public static IServiceCollection AddFulfillmentClient(this IServiceCollection services,
            Action<SecuredFulfillmentClientConfiguration> configureOptions)
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

            services.TryAddScoped<IFulfillmentClient, FulfillmentClient>();

            return services;
        }

        public static IServiceCollection AddWebhookProcessor(this IServiceCollection services)
        {
            services.TryAddScoped<IWebhookProcessor, WebhookProcessor>();
            return services;
        }

        public static void WithAzureTableOperationsStore(this IServiceCollection services, string storageAccountConnectionString)
        {
            services.TryAddScoped<IOperationsStore>(s => new AzureTableOperationsStore(storageAccountConnectionString));
        }

        public static void WithOperationsStore<T>(this IServiceCollection services) where T : class, IOperationsStore
        {
            services.TryAddScoped<IOperationsStore, T>();
        }

        public static void WithWebhookHandler<T>(this IServiceCollection services) where T : class, IWebhookHandler
        {
            services.TryAddScoped<IWebhookHandler, T>();
        }
    }
}
