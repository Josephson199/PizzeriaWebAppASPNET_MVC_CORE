using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PizzeriaWebAppASPNET_MVC_CORE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace PizzeriaWebAppASPNET_MVC_CORE
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();
            services.AddTransient<EFDatabaseRepo>();
            
            var connection = @"Server=LAPTOP-8HO4BK3G\SQLEXPRESS;Database=Tomasos;Trusted_Connection=True;";
            services.AddDbContext<TomasosContext>(options => options.UseSqlServer(connection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSession();
            app.UseStaticFiles();
            app.UseStatusCodePages();
            //app.UseSession();
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
