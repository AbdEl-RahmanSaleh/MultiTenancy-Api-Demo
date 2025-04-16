using Microsoft.EntityFrameworkCore;
using MultiTenancyExample.Data;
using MultiTenancyExample.Services;
using MultiTenancyExample.Settings;

namespace MultiTenancyExample
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddTenancy(this IServiceCollection services,ConfigurationManager configuration)
        {
            services.AddScoped<ITenantService, TenantService>();


            services.Configure<TenantSettings>(configuration.GetSection(nameof(TenantSettings)));
            TenantSettings options = new();
            configuration.GetSection(nameof(TenantSettings)).Bind(options);

            var defaultProvider = options.Defaults.DBProvider;

            if (defaultProvider.ToLower() == "mssql")
            {
                services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer());
            }
            foreach (var tenant in options.Tenants)
            {
                var connectionString = tenant.ConnectionString ?? options.Defaults.ConnectionString;

                using var scope = services.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.SetConnectionString(connectionString);

                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }


            return services;
        }
    }
}
