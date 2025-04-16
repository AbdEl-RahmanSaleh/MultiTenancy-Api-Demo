using Microsoft.Extensions.Options;
using MultiTenancyExample.Settings;

namespace MultiTenancyExample.Services
{
    public class TenantService : ITenantService
    {

        private Tenant? _currentTenant;
        private HttpContext? _httpContext;
        private readonly TenantSettings _tenantSettings;


        public TenantService(IHttpContextAccessor contextAccessor,IOptions<TenantSettings> tenantSettings)
        {
            _httpContext = contextAccessor.HttpContext;
            _tenantSettings = tenantSettings.Value;

            if(_httpContext is not null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant",out var tenantId))
                {
                    SetCurrentTenant(tenantId);
                }
                else
                {
                    throw new Exception("No Tenant Provided!");
                }
            }
        }

        public string? GetConnectionString()
        {
            var currentConnectionString = _currentTenant is null
                       ? _tenantSettings.Defaults.ConnectionString
                       : _currentTenant.ConnectionString;

            return currentConnectionString;
        }

        public Tenant? GetCurrentTenant() => _currentTenant;
        

        public string? GetDatabaseProvider() => _tenantSettings.Defaults.DBProvider;


        private void SetCurrentTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.FirstOrDefault(t => t.TId == tenantId);

            if (_currentTenant is null)
            {
                throw new Exception("Invalid Tenant Id");
            }

            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
            }
        }
    }
}
