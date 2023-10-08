using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace TodoListBlazorWasm.Api.Extensions;

public static class HostExtension
{
    public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var service = scope.ServiceProvider;
            var logger = service.GetRequiredService<ILogger<TContext>>();
            var context = service.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                var retries = 10;
                var retry = Policy.Handle<SqlException>().WaitAndRetry(
                    retryCount: retries,
                    sleepDurationProvider: r => TimeSpan.FromSeconds(Math.Pow(2, r)),
                    onRetry: (e, t, r, c) => logger.LogWarning(e, "[{Prefix}] Exception {ExceptionType} with message {Message} detected on attempt {Retry} of {Retries}", nameof(TContext), e.GetType().Name, e.Message, r, retries)
                );

                retry.Execute(() => InvokeSeeder(seeder!, context, service));
                logger.LogInformation("Migrated database associated with context {DbContext}", typeof(TContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
            }
        }

        return host;
    }

    public static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext? context, IServiceProvider service) where TContext : DbContext
    {
        if (context is not null)
        {
            context.Database.Migrate();
            seeder(context, service);
        }
    }
}
