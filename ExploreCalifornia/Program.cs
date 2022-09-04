using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using ExploreCalifornia.Extensions;

namespace ExploreCalifornia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                    .ConfigureAzureKeyValue()
                    .Build()
                    .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)            
            //.ConfigureLogging(logging => {
            //    logging.AddAzureWebAppDiagnostics();
            //}).ConfigureServices(serviceCollection => 
            //serviceCollection.Configure<AzureFileLoggerOptions>(options =>
            //{
            //    options.FileName = "azure-diagnostics-";
            //    options.FileSizeLimit = 50 * 1024;
            //    options.RetainedFileCountLimit = 5;                
            //}).Configure<AzureBlobLoggerOptions>(options => {                         
            //      options.BlobName = "log.txt";
            //}))
            .ConfigureAppConfiguration((httpContext, builder) =>
            {                
                var env = httpContext.HostingEnvironment;
                builder.AddEnvironmentVariables();                
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {                
                webBuilder.UseStartup<Startup>();
            });
    }
}
