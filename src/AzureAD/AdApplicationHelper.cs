using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace SaaSFulfillmentClient.AzureAD
{
    public static class AdApplicationHelper
    {
        private const string authenticationEndpoint = "https://login.microsoftonline.com/";

        // Please see https://docs.microsoft.com/en-us/azure/marketplace/partner-center-portal/pc-saas-registration#get-a-token-based-on-the-azure-ad-app
        private const string marketplaceResourceId = "20e940b3-4c77-4b0b-9a53-9e16a1b010a7";

        public static async Task<string> GetAccessToken(SecuredFulfillmentClientConfiguration options)
        {
            var credential = new ClientCredential(options.AzureActiveDirectory.ClientId.ToString(), options.AzureActiveDirectory.AppKey);

            var authContext = new AuthenticationContext(authenticationEndpoint + options.AzureActiveDirectory.TenantId, false);

            var token = await authContext.AcquireTokenAsync(marketplaceResourceId, credential);
            
            return token.AccessToken;
        }
    }
}
