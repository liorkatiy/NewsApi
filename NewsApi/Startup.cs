using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsApi.Entities.Config;
using NewsApi.Entities.Interfaces;
using NewsApi.Logic;
using NewsApi.Logic.Managers;
using NewsApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using NewsApi.Logic.Logger;

namespace NewsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var allowCors = Configuration.GetSection("CorsAllowedOrigins").Get<string[]>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowedOrigins",
                                  builder =>
                                  {
                                      builder.WithOrigins(allowCors);
                                  });
            });

            services.AddControllers();
            services.AddMediatR(typeof(NewsService));

            var newsApiConfig = Configuration.GetSection("NewsApiConfig").Get<NewsApiConfig>();
            services.AddSingleton(typeof(NewsApiConfig), newsApiConfig);

            services.AddSingleton<INewsHttpClient, NewsHttpClient>();
            services.AddSingleton<INewsServiceLogger, NewsServiceLogger>();
            services.AddSingleton<IRepositoryManager, InMemoryRepositoryManager>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "News API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });        
        }
    }
}
