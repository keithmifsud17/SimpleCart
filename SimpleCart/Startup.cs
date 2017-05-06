using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleCart.Context;
using SimpleCart.Repositories;
using System;

namespace SimpleCart
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            var connection = Configuration.GetConnectionString("SimpleCartConnectionString");

            services.AddDbContext<SimpleCartContext>(options => options.UseSqlite(connection));

            services.AddMemoryCache();
            services.AddCors();
            services.AddMvc();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    scope.ServiceProvider.GetService<SimpleCartContext>().Database.Migrate();
                    scope.ServiceProvider.GetService<SimpleCartContext>().AddSampleData();
                }
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader());
            app.UseMvc();
        }
    }
}
