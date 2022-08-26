using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;

namespace ExploreCalifornia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging => {
                logging.AddAzureWebAppDiagnostics();

            }).ConfigureServices(serviceCollection => 
            serviceCollection.Configure<AzureFileLoggerOptions>(options =>
            {
                options.FileName = "azure-diagnostics-";
                options.FileSizeLimit = 50 * 1024;
                options.RetainedFileCountLimit = 5;                
            }).Configure<AzureBlobLoggerOptions>(options => {                         
                  options.BlobName = "log.txt";
            }))
            //.ConfigureAppConfiguration((httpContext, options) =>
            //{
            //    var env = httpContext.HostingEnvironment;
            //    options.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            //    options.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            //    options.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json"));
            //})
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
