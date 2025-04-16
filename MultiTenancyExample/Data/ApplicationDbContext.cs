using Microsoft.EntityFrameworkCore;
using MultiTenancyExample.Entities;
using MultiTenancyExample.Interfaces;
using MultiTenancyExample.Services;

namespace MultiTenancyExample.Data
{
    public class ApplicationDbContext : DbContext
    {

        public string TenantId { get; set; }
        private readonly ITenantService _tenantService;
        public ApplicationDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
            TenantId = _tenantService.GetCurrentTenant()?.TId;
        }
        public DbSet<Student> Students { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasQueryFilter(e => e.TenantId == TenantId);
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e =>e.State == EntityState.Added))
            {
                entry.Entity.TenantId = TenantId;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnectionString();

            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var dbProvider = _tenantService.GetDatabaseProvider();

                if (dbProvider?.ToLower() == "mssql")
                {
                    optionsBuilder.UseSqlServer(tenantConnectionString);
                }
            }
        }
    }
}
