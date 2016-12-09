using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Goyello.ITADApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
                
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
                // how to set authorization
                var mainPolicy = new AuthorizationPolicyBuilder()
                                        .AddRequirements()
                                        .RequireAuthenticatedUser()
                                        .Build();

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("default", mainPolicy);
                });

                // also register filter if you want to set it globally
                services.AddMvc()
                    .AddMvcOptions(options => 
                    {
                        options.Filters.Add(new AuthorizeFilter(mainPolicy));    
                    }) ...
            */

            // fluent configuration builder
            services.AddMvc()
                .AddJsonOptions(options => 
                {
                    // it would be nice to return camelCase properties to client
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            // build in memocy cache - add it if you'd like to
            services.AddMemoryCache();

            // use app settings read from json
            var settingFrom_appSettings = Configuration["MyExternalServiceUrl"];

            // add yours services to IoC container
            services.AddSingleton<IItemsService, ItemsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(builder =>
            {
                builder.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}");
            });

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    context.Response.StatusCode = 200;
                    await next();
                }
            });
        }
    }
}
