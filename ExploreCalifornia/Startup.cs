using ExploreCalifornia.DAL;
using ExploreCalifornia.Filters;
using ExploreCalifornia.Infrastructure;
using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Channels;

namespace ExploreCalifornia
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var demoUserValue = Configuration["demouser"];
            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddCssBundle("/css/bundle.css", "/css/**/*.css");
                pipeline.AddJavaScriptBundle("/lib/js/bundle.js", "/lib/**/*.js");
            });

            services.AddHsts(config =>
            {
                config.Preload = true;
            });

            services.AddHttpsRedirection(config =>
            {
                config.HttpsPort = 44370;
            });

            services.AddResponseCaching();
            services.AddResponseCompression(options => { options.EnableForHttps = true; });
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddTransient<FeatureToggles>();
            services.AddSingleton<FormattingService>();

            // BLOG DB 
            services.AddDbContextPool<BlogDbContext>((dbContext) =>
            {
                var connectoionString = Configuration.GetValue<string>("ConnectionStrings:BlogDbContext");
                dbContext.UseSqlServer(connectoionString);
            });

            // ASPNET USERS Identity
            services.AddDbContextPool<BlogIdentityDbContext>((dbContext) =>
            {
                var connectoionString = Configuration.GetValue<string>("ConnectionStrings:BlogIdentityDbContext");
                dbContext.UseSqlServer(connectoionString);
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<BlogIdentityDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(configure =>
                {
                    configure.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });

                options.AddPolicy("mypolicy", policy =>
                {
                    policy.WithOrigins("http://aaa.com").AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddScoped<SignInManager<User>>();
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.AddApplicationInsightsTelemetry(Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

            services.AddScoped<TimerFilter>();
            services.AddTransient<TimerManager>();
            services.AddHostedService<FileUploaderBackgroundService>();
            services.AddSingleton<Channel<FileUploadMessage>>(_ => Channel.CreateUnbounded<FileUploadMessage>());
            services.AddHostedService<FileUploaderHostedService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, FeatureToggles featureToggle, ILogger<Startup> logger)
        {
            //logger.Log(LogLevel.Information, "in startup class");

            
            app.UseExceptionHandler("/Error");
         
            //if (featureToggle.EnabledDeveloperExceptions)
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            if (!featureToggle.EnabledDeveloperExceptions)
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseWebOptimizer();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCompression();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id:int?}");
            });


        }
    }
}
