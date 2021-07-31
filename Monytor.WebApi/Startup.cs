﻿using Monytor.Core.Repositories;
using Monytor.Core.Services;
using Monytor.Domain.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monytor.Core.Configurations;
using System;
using Monytor.Domain.Services;

namespace Monytor.WebApi {
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SetupDatabase(services, Configuration);
            services.AddScoped<ISeriesService, SeriesService>();
            services.AddScoped<IViewCollectionService, ViewCollectionService>();
            services.AddScoped<ICollectorConfigService, CollectorConfigService>();
            services.AddCors(setup=> setup.AddPolicy("localhost", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
            services.AddControllers().AddNewtonsoftJson();
        }
        private static void SetupDatabase(IServiceCollection services, IConfiguration appConfig) {
            var storageProvider = appConfig.GetValue<StorageProvider>("storageProvider");
            
            switch (storageProvider) {
                case StorageProvider.PostgreSql:
                    Console.WriteLine($"Using {appConfig["storageProviderConnectionString"]}");
                    PostgreSQL.Bootstrapper.SetupDatabaseAndRegisterRepositories(services, appConfig["storageProviderConnectionString"]);
                    break;
                case StorageProvider.RavenDb:
                    Console.WriteLine($"Using {appConfig["database:url"]} , {appConfig["database:name"]}");
                    RavenDb.Bootstrapper.SetupDatabaseAndRegisterRepositories(services, appConfig["database:url"], appConfig["database:name"]);
                    break;
                default:
                    throw new NotSupportedException($"The configured value of the setting 'storageProvider' is not supported.");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<UnitOfWorkMiddleware>();
            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors("localhost");

            app.UseEndpoints(endpoints =>
           {
               //endpoints.MapRazorPages();
               endpoints.MapControllers();
           });
        }
    }
}
