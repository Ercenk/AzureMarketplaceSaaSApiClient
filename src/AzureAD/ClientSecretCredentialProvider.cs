namespace SaaSFulfillmentClient.AzureAD
{
    public class ClientSecretCredentialProvider : ICredentialProvider
    {
        public ClientSecretCredentialProvider(string clientSecret)
        {
            this.ClientSecret = clientSecret;
        }

        public string ClientSecret { get; }
    }
}
