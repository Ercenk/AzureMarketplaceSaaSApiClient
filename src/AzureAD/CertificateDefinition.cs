using System.Security.Cryptography.X509Certificates;

namespace SaaSFulfillmentClient.AzureAD
{
    public class CertificateCredentialProvider : ICredentialProvider
    {
        public StoreName CertificateStore { get; set; }
        public string CertificateThumprint { get; set; }
        public StoreLocation StoreLocation { get; set; }
    }
}
