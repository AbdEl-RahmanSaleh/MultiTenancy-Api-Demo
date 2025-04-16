using MultiTenancyExample.Settings;

namespace MultiTenancyExample.Services
{
    public interface ITenantService
    {
        string? GetDatabaseProvider();
        string? GetConnectionString();
        Tenant? GetCurrentTenant();
    }
}
