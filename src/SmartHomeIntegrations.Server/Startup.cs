using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SmartHomeIntegrations.Core.HomeAssistant;
using SmartHomeIntegrations.Core.Infrastructure;
using SmartHomeIntegrations.Core.Integrations;
using SmartHomeIntegrations.FaceRecognition;

namespace SmartHomeIntegrations.Server
{
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartHomeIntegrations.Server", Version = "v1" });
            });

            services.Configure<ServerSettings>(Configuration.GetSection("ServerSettings"));
            services.AddScoped(s => new HttpClient());
            services.AddSingleton<IFaceDetector, FaceDetector>();
            services.AddSingleton<ISmartHomeClient, SmartHomeClient>();
            services.AddScoped<IProductivityService, ProductivityService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartHomeIntegrations.Server v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
