using System;
using System.Threading;
using System.Threading.Tasks;

namespace SaaSFulfillmentClient.WebHook
{
    public interface IWebhookProcessor
    {
        Task ProcessWebhookNotificationAsync(WebhookPayload details, CancellationToken cancellationToken = default);
    }
}
