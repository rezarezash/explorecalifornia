using ExploreCalifornia.DAL;
using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace ExploreCalifornia
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            //Environment = webHostEnvironment;

            //var conf = new ConfigurationBuilder()
            //    .SetBasePath(Environment.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: false)
            //    .AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", optional: true)
            //    .AddJsonFile(@"Properties/launchSettings.json", optional: false);

            //conf.AddEnvironmentVariables();
            //Configuration = conf.Build();
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //var sslPort = Configuration.GetValue<int>("iisSettings:iisExpress:sslPort");
            services.AddControllersWithViews();

            services.AddTransient<FeatureToggles>();
            services.AddSingleton<FormattingService>();

            // BLOG DB 
            services.AddDbContext<BlogDbContext>((dbContext) =>
            {
                var connectoionString = Configuration.GetValue<string>("ConnectionStrings:BlogDbContext");
                dbContext.UseSqlServer(connectoionString);
            });

            // ASPNET USERS Identity
            services.AddDbContext<BlogIdentityDbContext>((dbContext) =>
            {
                var connectoionString = Configuration.GetValue<string>("ConnectionStrings:BlogIdentityDbContext");
                dbContext.UseSqlServer(connectoionString);
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<BlogIdentityDbContext>();

            services.AddScoped<SignInManager<User>>();



            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.AddApplicationInsightsTelemetry(Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,

            FeatureToggles featureToggle)
        {
            app.UseExceptionHandler("/Home/Error");

            if (featureToggle.EnabledDeveloperExceptions)
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id:int?}");
            });

            app.UseStaticFiles();
        }
    }
}
