using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TodoApiAssignment.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Cassandra.Mapping;

namespace TodoApiAssignment
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
            services.AddDbContext<TodoContext>(option =>
            option.UseSqlServer(Configuration.GetConnectionString("Connection")));

            services.AddControllers();

            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddSingleton<INoSqlDb, CassandraDb>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Middleware to log request and response times
            app.Use(async (context, next) =>
            {
                logger.LogInformation("Request arrived at: " + DateTime.Now);
                await next();
                logger.LogInformation("Response left at: " + DateTime.Now);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
