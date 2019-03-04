using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using IdentityServer4.AccessTokenValidation;
using Dealership.API.Controllers;
using Dealership.API.Repositories;

namespace Dealership.API
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;
        public static string DealershipConnectionString;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                //.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthentication("bearer")
                .AddJwtBearer()
                .AddIdentityServerAuthentication("bearer", options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.ApiName = "vehicles_api";
                    options.ApiSecret = "apisecret";
                    options.RequireHttpsMetadata = false; // DEV only!!
                });

            DealershipConnectionString = Configuration["ConnectionStrings:DealershipEntities"];
            services.AddDbContext<Entities.DealershipContext>(options =>
                options.UseSqlServer(DealershipConnectionString));

            services.AddScoped<IVehicleRepository, VehicleRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
