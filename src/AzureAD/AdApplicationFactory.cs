using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace SaaSFulfillmentClient.AzureAD
{
    public static class AdApplicationHelper
    {
        public static IConfidentialClientApplication GetApplication(SecuredFulfillmentClientConfiguration options,
            ICredentialProvider credentialProvider)
        {
            if (!(credentialProvider is ClientSercretCredentialProvider secretProvider))
            {
                throw new NotImplementedException(
                    "Current implementation supports AD Applications with client secrets only.");
            }

            return ConfidentialClientApplicationBuilder
                .Create(options.AzureActiveDirectory.ClientId)
                .WithClientSecret(secretProvider.ClientSecret)
                .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs)
                .Build();
        }

        public static async Task<string> GetBearerToken(IConfidentialClientApplication adApplication)
        {
            var scopes = new[] {"https://graph.microsoft.com/.default"};
            return $"Bearer {(await adApplication.AcquireTokenForClient(scopes).ExecuteAsync()).AccessToken}";
        }
    }
}
