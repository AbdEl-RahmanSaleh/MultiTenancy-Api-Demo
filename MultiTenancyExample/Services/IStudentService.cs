using MultiTenancyExample.Entities;

namespace MultiTenancyExample.Services
{
    public interface IStudentService
    {
        Task<Student> CreatedAsync(Student product);
        Task<Student?> GetByIdAsync(int id);
        Task<IReadOnlyList<Student>> GetAllAsync();
    }
}
