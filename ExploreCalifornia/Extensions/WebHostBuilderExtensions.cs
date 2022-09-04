using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Logging.AzureAppServices;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace ExploreCalifornia.Extensions
{
    public static class WebHostBuilderExtensions
    {
        public static IHostBuilder ConfigureAzureKeyValue(this IHostBuilder builder) =>            
           builder.ConfigureAppConfiguration((context, configurationBuilder) =>
           {
               var configuration = configurationBuilder.Build();
               var connectionString = "RunAs=App;AppId=9dbe3e7d-aebf-43bc-81cf-0736f3c92a3e;TenantId=600303c3-fd43-48f7-892a-de435270692d;AppKey=Zir8Q~Skdz72ShRQHudcd4xy5LNgFlzUkqCnacEU";
               var azureServiceTokenProvider = new AzureServiceTokenProvider(connectionString);
               var keyVaultClient = new KeyVaultClient(
                  new KeyVaultClient.AuthenticationCallback(
                     azureServiceTokenProvider.KeyVaultTokenCallback));
               configurationBuilder.AddAzureKeyVault(
                  configuration.GetValue<string>("KeyValue:VaultUrl"),
                  keyVaultClient, new DefaultKeyVaultSecretManager());
           });
    }
}
