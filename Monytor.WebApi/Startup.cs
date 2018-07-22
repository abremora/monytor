using Monytor.Core.Repositories;
using Monytor.Core.Services;
using Monytor.Domain.Services;
using Monytor.RavenDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client;
using Monytor.Infrastructure;

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
            var url = Configuration["database:url"];
            var databaseName = Configuration["database:name"];
            var store = RavenHelper.CreateStore(url, databaseName);

            services.AddSingleton<IDocumentStore>(store);
            services.AddTransient<ICollectorService, CollectorService>();
            services.AddTransient<ISeriesRepository, SeriesRepository>();
            services.AddTransient<IViewCollectionService, ViewCollectionService>();
            services.AddTransient<IViewCollectionRepository, ViewCollectionRepository>();

            services.AddCors();
            services.AddMvc();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
