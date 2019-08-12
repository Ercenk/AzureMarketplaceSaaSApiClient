using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SaaSFulfillmentClient.AzureAD;

namespace SaaSFulfillmentClient
{
    public class FulfillmentClientBuilder
    {
        private readonly IServiceCollection services;

        public FulfillmentClientBuilder(IServiceCollection services)
        {
            this.services = services;
        }

        public void WithCertificateAuthentication(StoreLocation storeLocation, StoreName storeName,
            string certificateThumbprint)
        {
            this.services.TryAddSingleton<ICredentialProvider>(new CertificateCredentialProvider
            {
                StoreLocation = storeLocation,
                CertificateStore = storeName,
                CertificateThumprint = certificateThumbprint
            });
        }

        public void WithClientSecretAuthentication(string clientSecret)
        {
            this.services.TryAddSingleton<ICredentialProvider>(new ClientSecretCredentialProvider(clientSecret));
        }
    }
}
