using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace SaaSFulfillmentClient.AzureAD
{
    public static class AdApplicationHelper
    {
        private const string authenticationEndpoint = "https://login.microsoftonline.com/";

        // Please see https://docs.microsoft.com/en-us/azure/marketplace/partner-center-portal/pc-saas-registration#get-a-token-based-on-the-azure-ad-app
        private const string marketplaceResourceId = "62d94f6c-d599-489b-a797-3e10e42fbe22";

        public static async Task<string> GetAccessToken(SecuredFulfillmentClientConfiguration options)
        {
            var credential = new ClientCredential(options.AzureActiveDirectory.ClientId.ToString(), options.AzureActiveDirectory.AppKey);

            var authContext = new AuthenticationContext(authenticationEndpoint + options.AzureActiveDirectory.TenantId, false);

            var token = await authContext.AcquireTokenAsync(marketplaceResourceId, credential);
            
            return token.AccessToken;
        }
    }
}
