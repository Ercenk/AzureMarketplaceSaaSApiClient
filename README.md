# Azure Marketplace SaaS Fulfillment API Client

An experimental .NET client implementation for the Azure Marketplace SaaS Fulfillment API V2. 
I built this project as a part of a sample I am developing for demonstrating how SaaS applications can be integrated with Azure Marketplace.

The Azure SaaS Fulfillment API V2 reference is: [here](https://docs.microsoft.com/en-us/azure/marketplace/cloud-partner-portal/saas-app/cpp-saas-fulfillment-api-v2#update-a-subscription).

There is also a Postman collection showing the mock API.

This client is based on the mock API referenced in the article above.

The client is also available as a Nuget package at https://www.nuget.org/packages/AzureMarketplaceSaaSApiClient/

### **Breaking changes for version 2.0.0**
- Incorporated Azure AD
- Changed the interface on the client to remove the external bearer token. The client now implements Azure AD authentication.
- Currently only app secret (AppKey) is implemented (no certificate authentication)

### **New functionality**
- Added Web Hook processing capability