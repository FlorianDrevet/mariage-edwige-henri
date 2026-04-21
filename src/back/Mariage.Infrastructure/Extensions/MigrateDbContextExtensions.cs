using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InfraFlowSculptor.Infrastructure.Extensions;

internal static class MigrateDbContextExtensions
{
    private static readonly string ActivitySourceName = "DbMigrations";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services)
        where TContext : DbContext
        => services.AddMigration<TContext>((_, _) => Task.CompletedTask);

    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services, Func<TContext, IServiceProvider, Task> seeder)
        where TContext : DbContext
    {
        // Enable migration tracing
        services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource(ActivitySourceName));

        return services.AddHostedService(sp => new MigrationHostedService<TContext>(sp, seeder));
    }

    public static IServiceCollection AddMigration<TContext, TDbSeeder>(this IServiceCollection services)
        where TContext : DbContext
        where TDbSeeder : class, IDbSeeder<TContext>
    {
        services.AddScoped<IDbSeeder<TContext>, TDbSeeder>();
        return services.AddMigration<TContext>((context, sp) => sp.GetRequiredService<IDbSeeder<TContext>>().SeedAsync(context));
    }

    private static async Task MigrateDbContextAsync<TContext>(this IServiceProvider services, Func<TContext, IServiceProvider, Task> seeder) where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var scopeServices = scope.ServiceProvider;
        var logger = scopeServices.GetRequiredService<ILogger<TContext>>();
        var context = scopeServices.GetRequiredService<TContext>();

        using var activity = ActivitySource.StartActivity($"Migration operation {typeof(TContext).Name}");

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

            var strategy = context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(() => InvokeSeeder(seeder, context, scopeServices));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

            throw;
        }
    }

    private static async Task InvokeSeeder<TContext>(Func<TContext, IServiceProvider, Task> seeder, TContext context, IServiceProvider services)
        where TContext : DbContext
    {
        using var activity = ActivitySource.StartActivity($"Migrating {typeof(TContext).Name}");

        try
        {
            await ReconcileExistingSchemaAsync(context);
            await context.Database.MigrateAsync();
            await seeder(context, services);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    /// <summary>
    /// Handles the case where the database schema already exists but the migration history is incomplete.
    /// This occurs when the InitialCreate migration was recreated (e.g., SQL Server → PostgreSQL migration
    /// reset) and the database already has tables from a previous InitialCreate run.
    /// If "Gifts" table exists but InitialCreate is not recorded, we insert the record so MigrateAsync
    /// only applies incremental migrations (e.g., AddAccommodation).
    /// </summary>
    private static async Task ReconcileExistingSchemaAsync(DbContext context)
    {
        var connection = context.Database.GetDbConnection();
        var wasOpen = connection.State == System.Data.ConnectionState.Open;
        if (!wasOpen) await connection.OpenAsync();

        try
        {
            await using var cmd = connection.CreateCommand();

            // Ensure the migration history table exists before querying it
            cmd.CommandText = """
                CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
                    "MigrationId" character varying(150) NOT NULL,
                    "ProductVersion" character varying(32) NOT NULL,
                    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
                )
                """;
            await cmd.ExecuteNonQueryAsync();

            // Check if the base schema is already applied (Gifts table exists)
            cmd.CommandText = """
                SELECT COUNT(1) FROM information_schema.tables
                WHERE table_schema = 'public' AND table_name = 'Gifts'
                """;
            var giftsTableExists = (long)(await cmd.ExecuteScalarAsync())! > 0;

            if (giftsTableExists)
            {
                // Base schema exists — ensure InitialCreate is recorded to avoid re-applying it
                cmd.CommandText = """
                    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
                    SELECT '20260420170922_InitialCreate', '10.0.5'
                    WHERE NOT EXISTS (
                        SELECT 1 FROM "__EFMigrationsHistory"
                        WHERE "MigrationId" = '20260420170922_InitialCreate'
                    )
                    """;
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (!wasOpen) await connection.CloseAsync();
        }
    }

    private class MigrationHostedService<TContext>(IServiceProvider serviceProvider, Func<TContext, IServiceProvider, Task> seeder)
        : BackgroundService where TContext : DbContext
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return serviceProvider.MigrateDbContextAsync(seeder);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
public interface IDbSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(TContext context);
}
