using MultiTenancyExample.Interfaces;

namespace MultiTenancyExample.Entities
{
    public class Student : IMustHaveTenant
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = null!;
        public string Age { get; set; } = null!;
        public string Grade { get; set; } = null!;
        public string TenantId { get; set; } = null!;
    }
}
