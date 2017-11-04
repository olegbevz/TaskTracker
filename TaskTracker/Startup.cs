using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.DataAccess.Interfaces;
using TaskTracker.Extensions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TaskTracker
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
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddSingleton<ISubscriptionDictionary, SubscriptionDictionary>();
            services.AddTransient<SubscriptionScheduler>();
            services.AddDbContext<TaskTrackerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TaskTrackerContext")));

            services.AddSignalR();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });

                loggerFactory.AddDebug();
                loggerFactory.AddConsole();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.EnsureMigrationOfContext<TaskTrackerContext>();
            
            app.UseStaticFiles();

            app.StartSubscriptionScheduler();

            app.UseSignalR(routes =>
            {
                routes.MapHub<TaskHub>("task");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
