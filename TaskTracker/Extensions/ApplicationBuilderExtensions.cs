using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TaskTracker.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void EnsureMigrationOfContext<T>(this IApplicationBuilder app) where T : DbContext
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<T>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public static void StartSubscriptionScheduler(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var scheduler = serviceScope.ServiceProvider.GetService<SubscriptionScheduler>();
                scheduler.Start();
            }
        }
    }
}
